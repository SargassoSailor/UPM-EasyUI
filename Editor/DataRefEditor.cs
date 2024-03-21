using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;
using EUI;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Unity.Properties;

[CustomPropertyDrawer(typeof(DataRef))]
[CanEditMultipleObjects]
public class DataRefEditor : PropertyDrawer
{
    GameObject obj;
    private static string[] deprecatedProps = { "audio", "rigidbody2D", "parent", "rigidbody", "particleSystem", "collider", "collider2D", "renderer", "constantForce", "light", "animation", "camera", "hingeJoint", "networkView" };

    SerializedProperty prop;
    private List<string> GetProps(Type c)
    {
        if (c == null)
        {
            return new List<string>();
        }
        PropertyInfo[] props = c.GetProperties();
        List<string> propNames = new List<string>();

        foreach (PropertyInfo prop in props)
        {
            if (deprecatedProps.Contains(prop.Name)) { continue; }
            propNames.Add($"p-{prop.Name}");
        }
        return propNames;
    }

    private List<string> GetFields(Type c)
    {
        if (c == null)
        {
            return new List<string>();
        }
        FieldInfo[] fields = c.GetFields(BindingFlags.Public |
                                          /*BindingFlags.NonPublic |*/
                                          BindingFlags.Instance);
        List<string> propNames = new List<string>();

        foreach (FieldInfo field in fields)
        {
            if (deprecatedProps.Contains(field.Name)) { continue; }
            propNames.Add($"f-{field.Name}");
        }
        return propNames;
    }

    public DropdownField ShowVEPopup(string label, string serialName, List<string> options)
    {
        SerializedProperty idx = prop.FindPropertyRelative(serialName);
        int index = idx.intValue;

        if (index > options.Count)
        {
            index = 0;
        }
        idx.intValue = index;
        var dropdown = new DropdownField(label, options, index);
        
        dropdown.RegisterValueChangedCallback(evt =>
        {
            //SerializedProperty idx2 = prop.FindPropertyRelative(serialName);
            idx.intValue = dropdown.index;
            //this.compIdx.intValue = dropdown.index;
            idx.serializedObject.ApplyModifiedProperties();
        });
        return dropdown;
    }

    public List<string> PopulateFields(Type c)
    {

        List<string> propNames = new List<string>();
        propNames.AddRange(GetFields(c));
        propNames.AddRange(GetProps(c));
        //need to bind a refresh to re-get the properties

        return propNames;
    }

    public Type GetTypeFromName(object c, string name)
    {
        Type subObj = null;
        if (DataRef.isProperty(name))
        {
            subObj = c.GetType().GetProperty(name.Substring(2)).PropertyType;
        }
        else
        {
            subObj = c.GetType().GetField(name.Substring(2)).FieldType;
        }
        return subObj;
    }

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        prop = property;
        var container = new VisualElement();

        var label = new Label("Reference:");
        var objField = new PropertyField(property.FindPropertyRelative("objectToTrack"));
        //SingleAssetLoader.Load<VisualTreeAsset>("Wall").CloneTree(result);

        obj = property.FindPropertyRelative("objectToTrack")?.objectReferenceValue as GameObject;
        int varIdx = property.FindPropertyRelative("varIdx").intValue;

        DataRef refObj = property.boxedValue as DataRef;

        container.Add(label);
        container.Add(objField);

        if (obj != null)
        {
            Component[] comps = obj.GetComponents<Component>();
            List<string> compNames = new List<string>();
            List<string> propNames = new List<string>();
            List<string> subPropNames = new List<string>();
            foreach (Component comp in comps)
            {
                if (comp == null) { continue; }//check for invalid components. Itll pass then break the sub var listing
                compNames.Add(comp.GetType().ToString());
            }
            var componentSelect = ShowVEPopup("Component", "compIdx", compNames);
            Component c = comps[componentSelect.index];

            propNames = PopulateFields(c.GetType());

            var propertySelect = ShowVEPopup("Property", "varIdx", propNames);

            subPropNames.Add("None");
            subPropNames.AddRange(PopulateFields(GetTypeFromName(c, propNames[varIdx]))); 

            var subPropSelect = ShowVEPopup("SubProperty", "subVarIdx", subPropNames);


            container.Add(componentSelect);
            container.Add(propertySelect);
            container.Add(subPropSelect);

            componentSelect.RegisterValueChangedCallback(evt =>
            {
                c = comps[componentSelect.index];
                List<string> propNames = PopulateFields(c.GetType());

                SerializedProperty selComp = property.FindPropertyRelative("selComp");
                selComp.objectReferenceValue = c;
                selComp.serializedObject.ApplyModifiedProperties();

                propertySelect.choices = propNames;
                propertySelect.index = 0;
            });

            propertySelect.RegisterValueChangedCallback(evt =>
            {
                subPropNames = new List<string>();
                c = comps[componentSelect.index];
                subPropNames.Add("None");

                subPropNames.AddRange(PopulateFields(GetTypeFromName(c, evt.newValue)));

                subPropSelect.choices = subPropNames;

                SerializedProperty selVar = property.FindPropertyRelative("selVar");
                selVar.stringValue = evt.newValue;
                selVar.serializedObject.ApplyModifiedProperties();

                subPropSelect.index = 0;
            });

            subPropSelect.RegisterValueChangedCallback(evt =>
            {
                SerializedProperty selSubVar = property.FindPropertyRelative("selSubVar");
                selSubVar.stringValue = evt.newValue;
                selSubVar.serializedObject.ApplyModifiedProperties();

                subPropSelect.index = 0;
            });



        }
        /*else if (selComp.objectReferenceValue != null)
        {
            EditorGUILayout.LabelField($"Component: {selComp.objectReferenceValue.GetType().ToString()}");
            EditorGUILayout.LabelField($"Property: {selVar.stringValue}");
        }*/


        return container;
    }
}
