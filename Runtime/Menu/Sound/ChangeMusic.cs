using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EUI;

public class ChangeMusic : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip newMusic;

    private void OnEnable()
    {
        MenuManager.ins.changeMusic(newMusic);
    }

}
