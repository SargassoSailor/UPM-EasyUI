using UnityEngine;

public class LoadPreferences : MonoBehaviour
{
    public setAudioLevels[] channels;
    private void Start()
    {
        foreach (setAudioLevels channel in channels)
        {
            channel.Init();
        }

    }

}
