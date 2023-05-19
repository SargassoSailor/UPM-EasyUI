using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using EUI;
using UnityEngine.InputSystem;

public class SimplePause : MonoBehaviour
{
    private bool isPaused;								//Boolean to check if the game is paused or not
    public string pausePanelName = "PausePanel";
    public bool showMenuWhenPaused = true;
#if NEW_INPUT
    public InputAction pauseKeybinds;
#endif
    private void Start()
    {
#if NEW_INPUT
        pauseKeybinds.Enable();
        pauseKeybinds.performed += DoPause;
#endif
    }

    // Update is called once per frame
    void Update()
    {
#if !NEW_INPUT
        //InputSystem.
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
#endif
    }
#if NEW_INPUT
    public void DoPause(InputAction.CallbackContext inp)
    {
         DoPause(!isPaused);
    }
#endif
    public void DoPause(bool pause = true)
    {
        bool pauseCompleted = GameMGR.SetPause(pause);
        isPaused = pause;
        if (showMenuWhenPaused && pauseCompleted)
        {
            MenuManager.setPanel(pausePanelName, pause);
        }
    }

    public void UnPause()
    {
        DoPause(false);
    }

}
