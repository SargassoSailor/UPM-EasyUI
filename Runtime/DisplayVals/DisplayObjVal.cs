using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DisplayObjVal : MonoBehaviour
{
    //[HideInInspector]
    public GameObject objectToTrack;
    [HideInInspector]
    public Component selComp;
    [HideInInspector]
    public string selVar;
    [HideInInspector]
    public string selSubVar;
    private TextMeshProUGUI text;
    [HideInInspector]
    public Type compType;

    //used by custom inspector
    [HideInInspector]
    public int compIdx = 0;
    [HideInInspector]
    public int varIdx = 0;
    [HideInInspector]
    public int subVarIdx;

    public bool showLabel = false;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        if(selComp)
        {
            compType = selComp.GetType();
        }
    }

    private void OnValidate()
    {
        
    }

    public void SetTrackedObj(GameObject obj)
    {
        objectToTrack = obj;
        if(selComp)
        {
            selComp = obj.GetComponent(compType);
        }
        
    }

    public static bool isProperty(string name)
    {
        string varType = name.Substring(0, 2);
        string varName = name.Substring(2);
        string val = "";
        if (varType == "p-")
        {
            return true;
        }
        else if (varType == "f-")
        {
            return false;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if(selComp!=null && selVar != "")
        {
            string varType = selVar.Substring(0, 2);
            string varName = selVar.Substring(2);
            string val = "null";
            object obj = null;
            if(varType == "p-")
            {
                //varName = selComp.GetType().GetProperty(varName).Name;
                obj = selComp.GetType().GetProperty(varName).GetValue(selComp);
            }
            else if(varType == "f-")
            {
                //varName = selComp.GetType().GetField(varName).Name;
                obj = selComp.GetType().GetField(varName).GetValue(selComp);
            }

            if (subVarIdx > 0)
            {
                string subVarType = selSubVar.Substring(0, 2);
                string subVarName = selSubVar.Substring(2);
                varName += $".{subVarName}";

                object subObj = null;
                if (subVarType == "p-")
                {
                    //varName = selComp.GetType().GetProperty(varName).Name;
                    subObj = obj.GetType().GetProperty(subVarName).GetValue(obj);
                }
                else if (subVarType == "f-")
                {
                    //varName = selComp.GetType().GetField(varName).Name;
                    subObj = obj.GetType().GetField(subVarName).GetValue(obj);
                }

                if (subObj != null) { val = subObj.ToString(); }

            }
            else
            {
                if (obj != null) { val = obj.ToString(); }
            }


            //get field for variables?
            //why is this not saving
            //text.text = selComp.GetType().GetProperty(selVar).GetValue(selComp).ToString();
            string label = "";
            if (showLabel) { label = varName + ":"; }
            
            text.text = $"{label}{val}";
        }
        
    }
}
