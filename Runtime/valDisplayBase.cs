using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class valDisplayBase : MonoBehaviour
{
    private Text txt;
    private TMPro.TextMeshProUGUI txtmesh;
    public string var;
    public bool runOnce = false;
    // Use this for initialization
    void Start()
    {
        txt = gameObject.GetComponent<Text>();
        txtmesh = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        //autosize text element for scrolling?
    }

    //set up update event.
    void Update()
    {
        if (txt != null)
        {
            txt.text = GetVal(var);
        }
        if (txtmesh != null)
        {
            txtmesh.text = GetVal(var);
        }
        if (runOnce) { this.enabled = false; }
        
    }

    public virtual string GetVal(string var)
    {
        return "";
    }

}
