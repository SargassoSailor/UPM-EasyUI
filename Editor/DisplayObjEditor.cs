using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//C# Example (LookAtPointEditor.cs)
using UnityEditor;
using System;
using System.Reflection;
using NUnit.Framework.Internal;

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

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(objectToTrack);
        DrawDefaultInspector();

        compIndex = compIdx.intValue;
        varIndex = varIdx.intValue;
        
        if(obj != null)
        {
            Component[] comps = obj.GetComponents<Component>();
            List<string> compNames = new List<string>();
            foreach (Component comp in comps)
            {
                if(comp == null) { continue; }//check for invalid components. Itll pass then break the sub var listing
                compNames.Add(comp.GetType().ToString());
            }
            compList = compNames.ToArray();

            compIndex = EditorGUILayout.Popup(compIndex, compList);
            compIdx.intValue = compIndex;

            Component c = comps[compIndex];
            PropertyInfo[] props = c.GetType().GetProperties();
            List<string> propNames = new List<string>();
            foreach (PropertyInfo prop in props)
            {
                propNames.Add(prop.Name);
            }
            varList = propNames.ToArray();

            varIndex = EditorGUILayout.Popup(varIndex, varList);
            varIdx.intValue = varIndex;

            DisplayObjVal valDisplay = (DisplayObjVal)target;
            valDisplay.selVar = propNames[varIndex];
            valDisplay.selComp = comps[compIndex];

            selComp.objectReferenceValue = comps[compIndex];
            selVar.stringValue = propNames[varIndex];

            
        }
        

        serializedObject.ApplyModifiedProperties();
    }
}