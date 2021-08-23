using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleButton : MonoBehaviour
{
    public ButtonProps settings;
    void Start()
    {
        Button b = GetComponent<Button>();
        settings.InitButton(b);
    }
}
