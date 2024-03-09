using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DisplayGameModuleVals : MonoBehaviour
{
    [HideInInspector]
    public string selVar;
    private TextMeshProUGUI text;
    [HideInInspector]
    public Type compType;

    //used by custom inspector
    [HideInInspector]
    public int compIdx = 0;
    [HideInInspector]
    public int propIdx = 0;
    public bool showLabel = false;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnValidate()
    {

    }

    public void SetTrackedObj(GameObject obj)
    {
        //objectToTrack = obj;
        //selComp = obj.GetComponent(compType);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (selComp != null && selVar != "")
        {
            //get field for variables?
            //why is this not saving
            //text.text = selComp.GetType().GetProperty(selVar).GetValue(selComp).ToString();
            string label = "";
            if (showLabel) { label = selComp.GetType().GetProperty(selVar).Name + ":"; }
            string val = selComp.GetType().GetProperty(selVar).GetValue(selComp).ToString();

            text.text = $"{label}{val}";
        }*/

    }
}