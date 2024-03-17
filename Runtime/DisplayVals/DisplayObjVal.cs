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
    private TextMeshProUGUI text;
    [HideInInspector]
    public Type compType;

    //used by custom inspector
    [HideInInspector]
    public int compIdx = 0;
    [HideInInspector]
    public int varIdx = 0;
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

    // Update is called once per frame
    void Update()
    {
        if(selComp!=null && selVar != "")
        {
            string varType = selVar.Substring(0, 2);
            string varName = selVar.Substring(2);
            string val = "";
            if(varType == "p-")
            {
                //varName = selComp.GetType().GetProperty(varName).Name;
                val = selComp.GetType().GetProperty(varName).GetValue(selComp).ToString();
            }
            else if(varType == "f-")
            {
                //varName = selComp.GetType().GetField(varName).Name;
                val = selComp.GetType().GetField(varName).GetValue(selComp).ToString();
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
