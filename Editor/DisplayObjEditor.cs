using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//C# Example (LookAtPointEditor.cs)
using UnityEditor;
using System;
using System.Reflection;
using NUnit.Framework.Internal;
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
    GameObject obj;
    int compIndex;
    string[] compList;
    int varIndex;
    string[] varList;
    private static string[] deprecatedProps = { "audio", "rigidbody2D", "rigidbody", "particleSystem", "collider", "collider2D", "renderer", "constantForce", "light", "animation", "camera", "hingeJoint", "networkView" };

    void OnEnable()
    {
        //have displayobjval onvalidate create list.
        objectToTrack = serializedObject.FindProperty("objectToTrack");
        selComp = serializedObject.FindProperty("selComp");
        selVar = serializedObject.FindProperty("selVar");
        compIdx = serializedObject.FindProperty("compIdx");
        varIdx = serializedObject.FindProperty("varIdx");
        obj = objectToTrack.objectReferenceValue as GameObject;
    }

    private List<string> GetProps(object c)
    {
        PropertyInfo[] props = c.GetType().GetProperties();
        List<string> propNames = new List<string>();

        foreach (PropertyInfo prop in props)
        {
            if (deprecatedProps.Contains(prop.Name)) { continue; }
            propNames.Add($"p-{prop.Name}");
        }
        return propNames;
    }

    private List<string> GetFields(object c)
    {
        FieldInfo[] fields = c.GetType().GetFields(BindingFlags.Public |
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
            propNames.AddRange(GetFields(c));
            propNames.AddRange(GetProps(c));

            if (varIndex > propNames.Count)
            {
                varIndex = 0;
            }
            varList = propNames.ToArray();
            EditorGUILayout.LabelField("Property:");
            varIndex = EditorGUILayout.Popup(varIndex, varList);
            varIdx.intValue = varIndex;

            DisplayObjVal valDisplay = (DisplayObjVal)target;
            
            valDisplay.selVar = propNames[varIndex];
            valDisplay.selComp = comps[compIndex];

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