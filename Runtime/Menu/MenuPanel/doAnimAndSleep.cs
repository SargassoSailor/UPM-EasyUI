﻿using System;
using UnityEngine;

public class doAnimAndSleep : MonoBehaviour
{
    public event Action onSleep;
    public string animTrigger;
    public Animator anim;

    private AnimationEvent myEvent;
    private static float waitForClipDelay = 0.08f;

    // Use this for initialization

    public void doAnim()
    {

        myEvent = new AnimationEvent();
        myEvent.functionName = "myEventFunction";

        anim.SetTrigger(animTrigger);

        Invoke("setSleep", waitForClipDelay);

    }

    public void setSleep()// this gets the length of the animation clip and sets the game object to disable itself when it ends.
    {
        AnimatorClipInfo[] currInfo = anim.GetCurrentAnimatorClipInfo(0);
        if (currInfo.Length < 1)
        {
            sleepAfterDelay();//no animation
            return;
        }
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        Invoke("sleepAfterDelay", currInfo[0].clip.length - waitForClipDelay);
    }

    public void sleepAfterDelay()
    {
        if(onSleep!= null)
        {
            onSleep.Invoke();
        }
        Debug.Log(name + "going to sleep");
        gameObject.SetActive(false);
    }


}
