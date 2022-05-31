using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using EUI;

public class SimplePause : MonoBehaviour
{
    private bool isPaused;								//Boolean to check if the game is paused or not
    public string pausePanelName = "PausePanel";
    public bool showMenuWhenPaused = true;
    bool canPause = false;

    public void SetPauseAbility(bool canPause)
    {
        this.canPause = canPause;
    }

    // Update is called once per frame
    void Update()
    {

        //Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
        if (Input.GetButtonDown("Cancel") && !isPaused && canPause) // fix this
        {
            //Call the DoPause function to pause the game
            DoPause(true);
        }
        //If the button is pressed and the game is paused and not in main menu
        else if (Input.GetButtonDown("Cancel") && isPaused)
        {
            //Call the UnPause function to unpause the game
            UnPause();
        }

    }

    public void DoPause(bool pause = true)
    {
        MenuPause(pause);

        if (showMenuWhenPaused)
        {
            MenuManager.returnInstance().setPanel(pausePanelName, pause);
        }
    }

    public void MenuPause(bool pause = true)
    {
        isPaused = pause;
        if (pause) { Time.timeScale = 0; }
        else { Time.timeScale = 1; }
    }

    public bool isGamePaused()
    {
        if (isPaused)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UnPause()
    {
        DoPause(false);
    }

}
