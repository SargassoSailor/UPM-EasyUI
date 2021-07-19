using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class EditorFieldProps
{
    public string label;
    public SerializedProperty prop;
    public string fieldName;
}

public class EditorPopupProps : EditorFieldProps
{
    public int index;
    public string[] choices;
}

public class EditorRowProps
{
    public Rect pos;
    public List<EditorFieldProps> props = new List<EditorFieldProps>();

    public EditorRowProps(Rect pos)
    {
        props = new List<EditorFieldProps>();
        this.pos = pos;
    }

    public void AddProp(SerializedProperty obj, string fieldName, string label = "none")
    {
        props.Add(new EditorFieldProps { fieldName = fieldName, prop = obj, label = label });
    }

    public void AddMenu(SerializedProperty obj, string fieldName, string[] choices, int curIdx, string label = "none")
    {
        props.Add(new EditorPopupProps { fieldName = fieldName, prop = obj, label = label, choices = choices, index = curIdx });
    }

    public void Draw()
    {
        pos.x -= 100;
        int width = (int)pos.width / props.Count;
        foreach (EditorFieldProps p in props)
        {
            EditorPropField(ref pos, p, width);
        }
    }

    public void EditorPropField(ref Rect pos, EditorFieldProps field, int width)
    {
        //none check is to allow for a passthrough of an object that has its own ongui call where it draws its own fields.
        pos.width = width;
        if (field.label != "none")
        {
            EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(field.label));
            pos.y += 16;
            pos.height -= 16;
        }
        SerializedProperty prop = field.prop.FindPropertyRelative(field.fieldName);
        if (prop == null) { return; }

        EditorGUI.PropertyField(pos, prop, GUIContent.none);
        pos.x += width;
        if (field.label != "none")
        {
            pos.y -= 16;
            pos.height += 16;
        }
    }

    public void EditorPopupMenu(ref Rect pos, EditorPopupProps field, int width)
    {
        field.index = EditorGUI.Popup(pos, field.index, field.choices);
        if (field.index >= field.choices.Length)
        {
            field.index = 0;
        }

        //field.prop.FindPropertyRelative(field.fieldName).objectReferenceValue
        /*if (obj != null)
        {
            string[] tables = obj.getTables().ToArray();
            
            property.FindPropertyRelative("tableName").stringValue = tables[index];
            property.FindPropertyRelative("ID").stringValue = obj.primaryKey;
        }*/
    }

}
