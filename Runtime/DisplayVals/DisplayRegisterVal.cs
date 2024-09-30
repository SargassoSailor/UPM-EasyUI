using EUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayRegisterVal : MonoBehaviour
{
    private TextMeshProUGUI text;

    public bool showLabel = false;
    [Space]
    public DataRef reference = new DataRef();


    public float updateDelay = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        if (reference.selComp)
        {
            reference.compType = reference.selComp.GetType();
        }
        if (updateDelay > 0)
        {
            InvokeRepeating("updateVals", updateDelay, updateDelay);
        }
        updateVals();
    }

    public void SetTrackedObj(GameObject obj)
    {
        reference.objectToTrack = obj;
        if (reference.selComp)
        {
            reference.selComp = obj.GetComponent(reference.compType);
        }
        updateVals();
    }


    void updateVals()
    {
        string val = reference.GetValue();

        //get field for variables?
        //why is this not saving
        //text.text = selComp.GetType().GetProperty(selVar).GetValue(selComp).ToString();
        string label = "";
        // if (showLabel) { label = reference.GetType( + ":"; }

        text.text = $"{label}{val}";
    }

    private void OnValidate()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        if (updateDelay == 0)
        {
            updateVals();
        }
    }
}
