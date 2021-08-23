using EUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//TODO: should have a 'EasyButton' script that can be manually put in that takes props.
public enum buttonFunction { changeMenu, startGame, Quit, setPref, Custom, GoBack, OpenWeb, Continue, popupMenu, stopGame };

// Custom serializable class
[Serializable]
public class ButtonProps
{
    public string name = "Name";
    public buttonFunction onPress = buttonFunction.startGame;
    public string argument;
    public Color32 color = Color.grey;
    public Color32 txtcolor = Color.black;
    public ProjectSoundClip AC;
    public UnityEvent ev;

    public void InitButton(Button button)
    {
        Transform label = button.transform.Find("Label");
        if (label != null)
        {
            Text t = label.GetComponent<Text>();
            if (t == null)
            {
                TMPro.TextMeshProUGUI txtmesh = label.GetComponent<TMPro.TextMeshProUGUI>();
                txtmesh.text = name;
                txtmesh.color = txtcolor;
            }
            else
            {
                t.text = name;
            }


            Transform center = button.transform.Find("ButtonCenter");
            if (center != null)
            {
                center.GetComponent<Image>().color = color;
            }
        }


        switch (onPress)
        {
            case buttonFunction.changeMenu:
                button.onClick.AddListener(() => MenuManager.ins.changeMenu(argument));
                button.onClick.AddListener(() => ProjectSettings.data.PlaySound(AC.audioName));
                break;

            case buttonFunction.GoBack:
                button.onClick.AddListener(() => MenuManager.ins.goBack());
                break;

            case buttonFunction.startGame:
                button.onClick.AddListener(() => MenuManager.ins.startGame());
                break;

            case buttonFunction.stopGame:
                button.onClick.AddListener(() => MenuManager.ins.StopGameplay());
                break;

            case buttonFunction.Continue:
                button.onClick.AddListener(() => MenuManager.ins.loadGame());
                break;

            case buttonFunction.Quit:
                button.onClick.AddListener(() => MenuManager.ins.quitGame());
                break;

            case buttonFunction.setPref:
                string[] splitArg = argument.Split(':');
                button.onClick.AddListener(() => PlayerPrefs.SetString(splitArg[0], splitArg[1]));
                button.onClick.AddListener(() => MenuManager.ins.processPrefs());
                break;
            case buttonFunction.Custom:
                button.onClick.AddListener(() => ev.Invoke());
                if (AC == null)
                {
                    button.onClick.AddListener(() => ProjectSettings.data.PlaySound(ProjectSettings.data.menuConfirm));
                }
                break;

            case buttonFunction.OpenWeb:
                button.onClick.AddListener(() => MenuManager.returnInstance().openWeb(argument));
                break;

            case buttonFunction.popupMenu:
                button.onClick.AddListener(() => MenuManager.ins.changeMenu(argument, "", true));
                button.onClick.AddListener(() => ProjectSettings.data.PlaySound(AC.audioName));
                break;
        }

        if (AC.audioName != "")
        {
            button.onClick.AddListener(() => ProjectSettings.data.PlaySound(AC.audioName));
        }
    }
}

public class populateButtons : MonoBehaviour
{

    public List<ButtonProps> props;
    public GameObject layoutGroup; // where to place generated buttons
    public GameObject prefab;//button prefab

    void Reset()
    {
        props = new List<ButtonProps>();
        props.Add(new ButtonProps());
        props[0].name = "Button";
        props[0].color = Color.grey;
        layoutGroup = gameObject;
        prefab = ProjectSettings.data.defaultButton;
    }
    // Start is called before the first frame update
    //TODO: create monobehaviour that allows for single button props button. 
    public static Button createButton(ButtonProps props, GameObject buttonObj, bool isPrefab = true, GameObject layoutGroup = null)
    {
        GameObject obj = buttonObj;
        if (isPrefab)
        {
            obj = Instantiate(buttonObj, layoutGroup.transform);
            obj.transform.parent = layoutGroup.transform;
            obj.name = props.name;

        }
        Button button = obj.GetComponent<Button>();
        //button.Select();
        obj.AddComponent<UIselectOnEnable>();

        props.InitButton(button);

        return button;
    }
    void Start()
    {
        GUIutil.clearChildren(layoutGroup.transform, "none", true);
        generateButtons();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (Transform child in transform)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (child != null)
                {
                    UnityEditor.Undo.DestroyObjectImmediate(child.gameObject);
                }
            };
        }
        generateButtons();
    }
#endif
    void generateButtons()
    {
        bool selected = false;
        if (prefab == null) { prefab = ProjectSettings.data?.defaultButton; }
        foreach (ButtonProps b in props)
        {
            Button btn = createButton(b, prefab, true, layoutGroup);
            if (!selected)
            {
                btn.Select();
                selected = true;
                btn.gameObject.AddComponent<UIselectOnEnable>();
            }
        }
    }
}
