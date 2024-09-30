using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//C# Example (LookAtPointEditor.cs)
using UnityEditor;
using System;
using System.Reflection;
using NUnit.Framework.Internal;
using System.Linq;
/*
[CustomEditor(typeof(DisplayGameModuleVals))]
[CanEditMultipleObjects]
public class DisplayModuleEditor : Editor
{
    SerializedProperty selVar;
    SerializedProperty propIdx;
    GameObject obj;
    int propIndex;
    string[] propList;
    int varIndex;
    string[] varList;
    void OnEnable()
    {
        selVar = serializedObject.FindProperty("selVar");
        propIdx = serializedObject.FindProperty("propIdx");
        
        //obj = GameMGR.gameModule as GameObject;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //EditorGUILayout.PropertyField(objectToTrack);
        DrawDefaultInspector();

        propIndex = propIdx.intValue;

        propList = GameMGR.GetModuleProperties().Keys.ToArray();

        propIndex = EditorGUILayout.Popup(propIndex, propList);
        propIdx.intValue = propIndex;

        DisplayGameModuleVals valDisplay = (DisplayGameModuleVals)target;
        valDisplay.selVar = propList[propIndex];

        selVar.stringValue = propList[propIndex];


        serializedObject.ApplyModifiedProperties();
    }
}*/