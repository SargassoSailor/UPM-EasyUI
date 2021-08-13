using UnityEngine;

public class ShowPanels : MonoBehaviour
{

    public GameObject DialogContainer;//container for dialog box windows that go over everything

    public string animInName;
    public string animOutName;
    //public GameObject parentPanel;

    //Call this function to activate and display the Options panel during the main menu

    public void setPanel(string panelName, bool enabled, bool dialog)
    {
        returnPanel(panelName, dialog).SetActive(enabled);
    }

    public GameObject returnPanel(string panelName, bool dialog = false)
    {
        if (dialog)
        {
            return DialogContainer.transform.Find(panelName).gameObject;
        }
        else
        {
            Transform panelObj = transform.Find(panelName);
            if (panelObj == null) { Debug.LogError("MenuMgr-No Panel Found:" + panelName); return null; }

            return panelObj.gameObject;
        }
    }

    public void showPanel(string panelName, bool animate = false, bool dialog = false)
    {
        GameObject panel = returnPanel(panelName, dialog);
        if (panel == null) { return; }
        setPanel(panelName, true, dialog);
        if (animate && panel.GetComponent<Animator>() != null)
        { panel.GetComponent<Animator>().SetTrigger(animInName); }
    }

    public void showPanel(GameObject panel, bool animate = false)
    {
        panel.SetActive(true);
    }

    public void hidePanel(GameObject panel, bool animate = false)
    {
        panel.SetActive(false);
    }

    public void hideP(string panel)
    {
        GameObject obj = returnPanel(panel);
        hidePanel(obj, true);
    }
    public void showP(string panel)
    {
        GameObject obj = returnPanel(panel);
        showPanel(panel, true, false);
    }

    public void hidePanel(string panelName, bool animate = false, bool dialog = false)
    {
        GameObject panel = returnPanel(panelName, dialog);
        if (panel == null) { return; }
        if (animate)
        {
            doAnimAndSleep anim = panel.GetComponent<doAnimAndSleep>();
            if (anim != null)
            {
                anim.doAnim();
            }
            else
            {
                setPanel(panelName, false, false);
            }

        }
        else
        {
            setPanel(panelName, false, false);
        }

    }

}
