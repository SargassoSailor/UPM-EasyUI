using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DisplayObjVal : MonoBehaviour
{
    public GameObject objectToTrack;
    public Component selComp;
    public string selVar;
    public TextMeshProUGUI text;
    public Type compType;

    //used by custom inspector
    public int compIdx = 0;
    public int varIdx = 0;
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
        selComp = obj.GetComponent(compType);
    }

    // Update is called once per frame
    void Update()
    {
        if(selComp!=null && selVar != "")
        {
            //get field for variables?
            //why is this not saving
            //text.text = selComp.GetType().GetProperty(selVar).GetValue(selComp).ToString();
            text.text = selComp.GetType().GetProperty(selVar).GetValue(selComp).ToString();
        }
        
    }
}
