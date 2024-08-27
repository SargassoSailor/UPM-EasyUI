using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PopulateDropdownFromRegister : MonoBehaviour
{
    [HideInInspector]
    public string chosenDropdownVal;
    [HideInInspector]
    public int chosenIndex;

    public UnityEvent<string> onDropdownSelected;
    private TMP_Dropdown dropdown;
    private List<string> options;

    public bool disableIfNoOptions = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SetupOptions", 1);
    }
    public void SetupOptions()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        List<string> optionList = DataRegister.GetFullOptions(chosenDropdownVal);
        options = optionList;
        dropdown.AddOptions(optionList);
        dropdown.onValueChanged.AddListener(OptionChanged);
        if (disableIfNoOptions)
        {
            Invoke("NoOptionsCheck", 0.1f);
        }
    }

    public void NoOptionsCheck()
    {
        if(dropdown.options.Count < 2)
        {
            if (dropdown.options.Count == 1)
            {
                OptionChanged(0);
            }
            gameObject.SetActive(false);
        }
    }

    void OptionChanged(int index)
    {
        string opt = options[index];
        onDropdownSelected.Invoke(opt);
    }

}
