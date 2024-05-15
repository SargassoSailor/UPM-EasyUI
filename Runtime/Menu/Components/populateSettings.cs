using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SettingsType
{
    Slider,
    Toggle,
    Text
}
[Serializable]
public class SettingsProps
{
    public string name;
    public SettingsType type;
    public Color32 color = Color.grey;
    public string prefName;
    public Vector2 sliderRange;
    public bool sliderWholeNumbers;
    public float defaultValue;

    public void InitItem(GameObject item)
    {
        Transform label = item.transform.Find("Label");
        Transform valueLabel = item.transform.Find("Value");
        SetPrefs prefsComponent = item.GetComponent<SetPrefs>();
        prefsComponent.prefName = prefName;
        if (label != null)
        {
            Text t = label.GetComponent<Text>();
            if (t == null)
            {
                TMPro.TextMeshProUGUI txtmesh = label.GetComponent<TMPro.TextMeshProUGUI>();
                txtmesh.text = name;
                txtmesh.color = color;
            }
            else
            {
                t.text = name;
            }
        }
        if(type == SettingsType.Slider)
        {
            Slider slider = item.GetComponentInChildren<Slider>();
            TMPro.TextMeshProUGUI valueTxt = valueLabel.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            slider.maxValue = sliderRange.y;
            slider.minValue = sliderRange.x;
            slider.wholeNumbers = sliderWholeNumbers; 
            slider.value = defaultValue;
            valueTxt.text = defaultValue.ToString();
            if (slider != null)
            {
                slider.onValueChanged.AddListener((float f) => prefsComponent.setValue(f));
                slider.onValueChanged.AddListener((float f) => valueTxt.text = f.ToString()) ;
            }
        }
        else if(type == SettingsType.Toggle)
        {
            Toggle toggle = item.GetComponent<Toggle>();
            if (defaultValue >= 1) { toggle.isOn = true; }
            if (toggle != null)
            {
                toggle.onValueChanged.AddListener((bool b) => prefsComponent.setValue(b));
            }
        }
    }
}

public class populateSettings : populateItems
{
    public GameObject togglePrefab;
    public GameObject sliderPrefab;
    public List<SettingsProps> props;

    public GameObject createItem(SettingsProps props, GameObject layoutGroup = null)
    {
        GameObject prefab = togglePrefab;
        if(props.type == SettingsType.Slider) { prefab = sliderPrefab; }

        GameObject obj = Instantiate(prefab, layoutGroup.transform);
        obj.transform.parent = layoutGroup.transform;
        obj.name = props.name;
        obj.hideFlags = HideFlags.DontSaveInEditor;
        props.InitItem(obj);
        return obj;
    }
    public override void generateItems()
    {
        bool selected = false;
        foreach (SettingsProps s in props)
        {
            GameObject obj = createItem(s, layoutGroup);
        }
    }
}

