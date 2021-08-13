using UnityEngine;
using UnityEngine.UI;
/// <summary>
// Display a projectValue in a gui element.
/// </summary>


public class displayProjVal : MonoBehaviour
{

    private Text txt;
    private TMPro.TextMeshProUGUI txtmesh;
    public string var;
    // Use this for initialization
    void Start()
    {
        txt = gameObject.GetComponent<Text>();
        txtmesh = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        //autosize text element for scrolling?
    }

    // Update is called once per frame
    void Update()
    {
        if (txt != null)
        {
            txt.text = ProjectSettings.returnStr(var);
        }
        if (txtmesh != null)
        {
            txtmesh.text = ProjectSettings.returnStr(var);
        }

        this.enabled = false;
    }

}
