using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//C# Example (LookAtPointEditor.cs)
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

[CustomEditor(typeof(DisplayObjVal))]
[CanEditMultipleObjects]
public class DisplayObjEditor : Editor
{
    SerializedProperty objectToTrack;
    SerializedProperty selComp;
    SerializedProperty selVar;
    SerializedProperty compIdx;
    SerializedProperty varIdx;
    SerializedProperty subVarIdx;
    SerializedProperty selSubVar;
    GameObject obj;
    int compIndex;
    string[] compList;
    int varIndex;
    int subVarIndex;
    string[] varList;
    private static string[] deprecatedProps = { "audio", "rigidbody2D", "parent","rigidbody", "particleSystem", "collider", "collider2D", "renderer", "constantForce", "light", "animation", "camera", "hingeJoint", "networkView" };

    void OnEnable()
    {
        //have displayobjval onvalidate create list.
        objectToTrack = serializedObject.FindProperty("objectToTrack");
        selComp = serializedObject.FindProperty("selComp");
        selVar = serializedObject.FindProperty("selVar");
        compIdx = serializedObject.FindProperty("compIdx");
        varIdx = serializedObject.FindProperty("varIdx");
        subVarIdx = serializedObject.FindProperty("subVarIdx");
        selSubVar = serializedObject.FindProperty("selSubVar");
        obj = objectToTrack.objectReferenceValue as GameObject;
    }

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
        if(c == null)
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

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(objectToTrack);
        
        //DrawDefaultInspector();

        compIndex = compIdx.intValue;
        varIndex = varIdx.intValue;
        subVarIndex = subVarIdx.intValue;

        obj = objectToTrack.objectReferenceValue as GameObject;

        if (obj != null)
        {
            EditorGUILayout.Space();
            Component[] comps = obj.GetComponents<Component>();
            List<string> compNames = new List<string>();
            foreach (Component comp in comps)
            {
                if(comp == null) { continue; }//check for invalid components. Itll pass then break the sub var listing
                compNames.Add(comp.GetType().ToString());
            }

            
            if (compIndex > comps.Length)
            {
                compIndex = 0;
            }

            compList = compNames.ToArray();
            EditorGUILayout.LabelField("Component:");

            compIndex = EditorGUILayout.Popup(compIndex, compList);
            compIdx.intValue = compIndex;

            Component c = comps[compIndex];

            List<string> propNames = new List<string>();
            propNames.AddRange(GetFields(c.GetType()));
            propNames.AddRange(GetProps(c.GetType()));

            if (varIndex > propNames.Count)
            {
                varIndex = 0;
            }
            varList = propNames.ToArray();

            List<string> subPropNames = new List<string>();

            Type subObj = null;

            //need to support if prop/field is null. make an instance?
            if(DisplayObjVal.isProperty(propNames[varIndex]))
            {
                subObj = c.GetType().GetProperty(propNames[varIndex].Substring(2)).PropertyType;
            }
            else
            {
                subObj = c.GetType().GetField(propNames[varIndex].Substring(2)).FieldType;
            }

            subPropNames.Add("None");
            subPropNames.AddRange(GetFields(subObj));
            subPropNames.AddRange(GetProps(subObj));

            EditorGUILayout.LabelField("Property:");
            varIndex = EditorGUILayout.Popup(varIndex, varList);
            varIdx.intValue = varIndex;

            if (subPropNames.Count > 1)
            {
                EditorGUILayout.LabelField("Sub Property:");
                if (subVarIndex > subPropNames.Count) { subVarIndex = 0; }
                subVarIndex = EditorGUILayout.Popup(subVarIndex, subPropNames.ToArray());

                
                selSubVar.stringValue = subPropNames[subVarIndex];
                subVarIdx.intValue = subVarIndex;
            }

            selComp.objectReferenceValue = comps[compIndex];
            selVar.stringValue = propNames[varIndex];
            

        }
        else if(selComp.objectReferenceValue != null)
        {
            EditorGUILayout.LabelField($"Component: {selComp.objectReferenceValue.GetType().ToString()}");
            EditorGUILayout.LabelField($"Property: {selVar.stringValue}");
        }
        

        bool changes = serializedObject.ApplyModifiedProperties();
        if (changes)
        {
            Repaint();
        }
        //EditorUtility.SetDirty(EditorWindow.focusedWindow);
        
    }
}