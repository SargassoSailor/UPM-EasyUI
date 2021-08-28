using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EUI;

public class FaderControl : MonoBehaviour
{
    [Tooltip("A value of 1 is approx 1.5 seconds")]
    public float fadeTimeMultiplier = 1f;
    private Animator anim;
    private float animTime;

    public void FadeOut(E_callback callback)
    {
        anim.SetTrigger("fadeOut");
        StartCoroutine(CompleteFade(callback));
    }
    public void FadeIn(E_callback callback)
    {
        anim.SetTrigger("fadeIn");
        StartCoroutine(CompleteFade(callback));
    }

    IEnumerator CompleteFade(E_callback callback)
    {
        yield return new WaitForSeconds(animTime);
        callback?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
        anim.SetFloat("fadeTime", fadeTimeMultiplier);
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        { 
            animTime = clipInfo[0].clip.length;
            animTime = animTime / fadeTimeMultiplier;
        }
        else
        {
            Debug.LogError("FadeControl- no animator clip set");
        }
        
    }

}
