using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using EUI;

public class DisplayObjVal : MonoBehaviour
{
    private TextMeshProUGUI text;

    public bool showLabel = false;
    [Space]
    public DataRef reference = new DataRef();
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        if(reference.selComp)
        {
            reference.compType = reference.selComp.GetType();
        }
    }

    // Update is called once per frame
    void Update()
    {
        string val = reference.GetValue();

        //get field for variables?
        //why is this not saving
        //text.text = selComp.GetType().GetProperty(selVar).GetValue(selComp).ToString();
        string label = "";
        //if (showLabel) { label = varName + ":"; }
            
        text.text = $"{label}{val}";
        
    }
}
