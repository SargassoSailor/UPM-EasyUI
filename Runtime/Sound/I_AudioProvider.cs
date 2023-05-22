using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_AudioProvider
{
    public void ChangeMusic(string musicName);
    public void PlaySound(string sfxName);
    public void Init(GameObject hostObj);

}
