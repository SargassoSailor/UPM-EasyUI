using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ProjectSoundClip))]
public class SoundClipDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    int _choiceIndex = 0;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        ProjectData dat = ProjectSettings.GetData();
        string[] audioList = dat.GetSoundList();
        if(audioList.Length !=0)
        {
            SerializedProperty soundStr = property.FindPropertyRelative("audioName");
            SerializedProperty soundIdx = property.FindPropertyRelative("idx");


            if (soundIdx.intValue >= audioList.Length) { soundIdx.intValue = audioList.Length - 1; }

            soundIdx.intValue = EditorGUI.Popup(position, soundIdx.intValue, audioList);

            soundStr.stringValue = dat.audioList[soundIdx.intValue].name;

        }

        EditorGUI.EndProperty();
    }

}
