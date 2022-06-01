using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPrefs : MonoBehaviour
{
    public string prefName;

    private void Start()
    {
        Toggle t = GetComponent<Toggle>();
        if (t) { t.isOn = Convert.ToBoolean((PlayerPrefs.GetInt(prefName))); }
        Slider s = GetComponentInChildren<Slider>();
        if (s) { s.value = PlayerPrefs.GetFloat(prefName); }
    }

    public void setValue(bool val)
    {
        PlayerPrefs.SetInt(prefName,val ? 1 : 0);
        PlayerPrefs.Save();
    }
    public void setValue(float val)
    {
        PlayerPrefs.SetFloat(prefName, val);
        PlayerPrefs.Save(); 
    }
}
