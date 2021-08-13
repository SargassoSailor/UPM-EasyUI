using UnityEngine;
using UnityEngine.UI;

public class UISound : MonoBehaviour
{
    [Header("Sound Clip")]
    public ProjectSoundClip soundClip;

    // Use this for initialization
    void Start()
    {
        Toggle t = GetComponent<Toggle>();
        if (t != null)
        {
            t.onValueChanged.AddListener(delegate { PlaySound(); });
        }

        Button b = GetComponent<Button>();
        if (b != null)
        {
            b.onClick.AddListener(() => PlaySound());
        }
    }

    public void PlaySound()
    {
        ProjectSettings.data.PlaySoundClip(soundClip);
    }

}