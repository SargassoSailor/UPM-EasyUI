using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//C# Example (LookAtPointEditor.cs)
using UnityEditor;
using System;
using System.Reflection;
#if UNITY_EDITOR
[CustomEditor(typeof(PopulateDropdownFromRegister))]
[CanEditMultipleObjects]
public class PopDropdownRegEditor : Editor
{
    SerializedProperty chosenDropdown;
    SerializedProperty chosenIdx;

    int cIndex;
    string[] cList;

    PopulateDropdownFromRegister script;
    void OnEnable()
    {
        //have displayobjval onvalidate create list.
        chosenDropdown = serializedObject.FindProperty("chosenDropdownVal");
        chosenIdx = serializedObject.FindProperty("chosenIndex");

        script = (PopulateDropdownFromRegister)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        cIndex = chosenIdx.intValue;

        string[] optList = DataRegister.GetAllOptionNames().ToArray();
        
        cIndex = EditorGUILayout.Popup(cIndex, optList);
        //chosenIdx.intValue = cIndex;
        //chosenDropdown.stringValue = optList[cIndex];
        //script.chosenIndex = cIndex;
        //script.chosenDropdownVal = optList[cIndex];
        
        if (cIndex != chosenIdx.intValue)
        {
            Debug.Log($"popdropdown - {cIndex}");
            chosenIdx.intValue = cIndex;
            chosenDropdown.stringValue = optList[cIndex];
            serializedObject.ApplyModifiedProperties();
        }
        DrawDefaultInspector();
        
    }
}
#endif