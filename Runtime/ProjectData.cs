using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
[System.Serializable]

public class ProjectTest
{
    [Header("Project Data")]
    public string gameName = "CarDerby";
    public string versionNum;
    //public MenuManager menu;
    [TextArea(6, 20)]
    public string credits = "";
    public GameObject defaultButton;
    [Header("Sound")]
    public List<AudioClip> audioList;
    private Dictionary<string, AudioClip> audio;
    public List<AudioMixerGroup> audioMixers;
    private Dictionary<string, AudioMixerGroup> mixers;
    [HideInInspector]
    public AudioClip menuMusic;
    public string menuConfirm;
    public string menuCancel;
    public string gameStart;
}
/// <summary>
/// This is an asset which contains all the data for a theme.
/// As an asset it live in the project folder, and get built into an asset bundle.
/// </summary>
[CreateAssetMenu(fileName = "projData", menuName = "UI/ProjectData")]
public class ProjectData : ScriptableObject
{
    [Header("Project Data")]
    public string gameName = "CarDerby";
    public string versionNum;
    //public MenuManager menu;
    [TextArea(6, 20)]
    public string credits = "";
    public GameObject defaultButton;
    [Header("Sound")]
    public List<AudioClip> audioList;
    private Dictionary<string, AudioClip> audio;
    public List<AudioMixerGroup> audioMixers;
    private Dictionary<string, AudioMixerGroup> mixers;
    [HideInInspector]
    public AudioClip menuMusic;
    public string menuConfirm;
    public string menuCancel;
    public string gameStart;
    [Header("Music")]
    public AudioClip gameMusic;

    [Header("Menus")]
    public string showInMenu;
    public string hideInMenu;
    public string showInGame;
    public string hideInGame;
    //each map should have its own music property.

    private void OnValidate()
    {
        SetupAudioList();
    }

    private void SetupAudioList()
    {
        audio = new Dictionary<string, AudioClip>();
        foreach (AudioClip a in audioList)
        {
            if (a == null || audio.ContainsKey(a.name)) { continue; }
            audio.Add(a.name, a);
        }
        mixers = new Dictionary<string, AudioMixerGroup>();
        foreach (AudioMixerGroup a in audioMixers)
        {
            if (a == null || mixers.ContainsKey(a.name)) { continue; }
            mixers.Add(a.audioMixer.name, a);
        }
    }

    private void OnEnable()
    {
        ProjectSettings.data = this;
        SetupAudioList();
    }

    public void PlaySound(string sound)
    {
        audio.TryGetValue(sound, out AudioClip clip);
        if (clip != null)
        {
            ProjectSettings.audioPlayer?.PlayOneShot(clip);
        }
    }

    public void PlaySoundClip(ProjectSoundClip clip)
    {
        PlaySound(clip.audioName);
    }

    public string[] GetSoundList()
    {
        return audio.Keys.ToArray();
    }

    public AudioMixerGroup GetMixer(string name)
    {
        bool found = mixers.TryGetValue(name, out AudioMixerGroup mixer);
        if(found)
        {
            return mixer;
        }
        else { return null; }
    }
}