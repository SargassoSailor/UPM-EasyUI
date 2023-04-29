using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using EUI;

//Old Pause, migrate volume code to simplepause
public class Pause : MonoBehaviour {


	private bool isPaused;								//Boolean to check if the game is paused or not
    public AudioMixer mainMixer;
    private float origVolume;
    public string pausePanelName = "PausePanel";
    public bool showMenuWhenPaused = true;
    public float pausedVolume = -50;
	
	//Awake is called before Start()
	void Awake()
	{
        mainMixer.GetFloat("musicVol",out origVolume);
        //Get a component reference to ShowPanels attached to this object, store in showPanels variable
		//Get a component reference to StartButton attached to this object, store in startScript variable
	}

	// Update is called once per frame
	void Update () {

        //Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
        if (Input.GetButtonDown("Cancel") && !isPaused && !BaseGM.inMenu && BaseGM.gameRunning) // fix this
		{
			//Call the DoPause function to pause the game
			DoPause(true);
		} 
		//If the button is pressed and the game is paused and not in main menu
		else if (Input.GetButtonDown ("Cancel") && isPaused) 
		{
			//Call the UnPause function to unpause the game
			UnPause ();
		}
	
	}


	public void DoPause(bool pause= true)
	{
        setPlayerControls(!pause);
		isPaused =  pause;

        if (pause) { Time.timeScale = 0; }
        else { Time.timeScale = 1; }

        if (showMenuWhenPaused)
        {
            if (pause) { mainMixer.SetFloat("musicVol", -50); }
            else { mainMixer.SetFloat("musicVol", origVolume); }
            MenuManager.setPanel(pausePanelName, pause);
        }
	}

    public void setPlayerControls(bool enabled)
    {
        /*if(playerControls != null)
        {
            playerControls.enabled = enabled;
        }*/
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
