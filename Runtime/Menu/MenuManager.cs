using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace EUI
{
    public delegate void E_callback();
    public class MenuManager : MonoBehaviour
    {

        private ShowPanels panels;

        public static MenuManager ins;

        [HideInInspector] public string currentPanel = "MenuPanel";
        private string prevPanel;

        public GameObject pausePanel;
        [HideInInspector] public string pausePanelName = "PausePanel";

        protected Dictionary<string, AudioClip> sounds;


        private AudioSource audio;
        private AudioSource music;

        public static bool gameRunning = false;
        public static bool dataLoaded { get; } = false;

        public GameObject detailsCard;
        public GameObject selectedItem;

        public UnityEvent startGameEvent;
        public UnityEvent continueGameEvent;
        public UnityEvent stopGameEvent;

        public bool changeSceneOnStart = true;
        public bool changeSceneOnEnd = false;//there is a bug here that causes bugs and a duplication? of MenuManager.
        public static bool isPaused = false;

        private GameObject msgPanel;
        private bool runStartEvent = true;

        public FaderControl fader;

        public bool HandleMusic = true;
        public GameObject returnCurrentPanel()
        {
            return panels.returnPanel(currentPanel);
        }
        public void quitGame()
        {
            Invoke("exit", 1);
            ProjectSettings.Data.PlaySound(ProjectSettings.data.menuCancel);
            panels.hidePanel(currentPanel, true);
        }

        public void exit()
        {
            Application.Quit();
        }

        private void OnLevelWasLoaded(int level)
        {
            if (MenuManager.ins != null && MenuManager.ins != this)
            {
                Destroy(gameObject);
            }
            else
            {
                MenuManager.ins = this;
            }
            audio = GetComponent<AudioSource>();
            panels = gameObject.GetComponent<ShowPanels>();
            ProjectSettings.audioPlayer = GetComponent<AudioSource>();

            string[] hideInMenu = { "" };
            string[] hideInGame = { "" };
            string[] showInGame = { "" };
            string[] showInMenu = { "" };

            if (ProjectSettings.data.showInMenu != "")
            {
                showInMenu = ProjectSettings.data.showInMenu.Split(',');
            }
            if (ProjectSettings.data.hideInMenu != "")
            {
                hideInMenu = ProjectSettings.data.hideInMenu.Split(',');
            }
            if (ProjectSettings.data.showInGame != "")
            {
                showInGame = ProjectSettings.data.showInGame.Split(',');
            }
            if (ProjectSettings.data.hideInGame != "")
            {
                hideInGame = ProjectSettings.data.hideInGame.Split(',');
            }
            if(level == 2) { level = 0; }

            switch (level)
            {
                case 0:
                    foreach (string s in hideInMenu)
                    {
                        if (s == "") { continue; }
                        panels.hidePanel(s);
                    }
                    foreach (string s in showInMenu)
                    {
                        if (s == "") { continue; }
                        panels.showPanel(s);
                    }
                    break;

                case 1:
                    foreach (string s in hideInGame)
                    {
                        if (s == "") { continue; }
                        panels.hidePanel(s);
                    }
                    foreach (string s in showInGame)
                    {
                        if (s == "") { continue; }
                        panels.showPanel(s);
                    }
                    break;
            }
            
        }

        //to be used to change menu in a unityevent
        public void changeMenuUnity(string panel)
        {
            changeMenu(panel);  
        }
        public void changeMenu(string panel, string pressSound = "", bool modalMenu = false)
        {
            if (pressSound == "")
            {
                pressSound = ProjectSettings.Data.menuConfirm;
            }
            ProjectSettings.Data.PlaySound(pressSound);
            prevPanel = currentPanel;
            if (!modalMenu && currentPanel!=panel)
            {
                panels.hidePanel(currentPanel, true);
            }
            else
            {
                //show modal bg.
            }

            panels.showPanel(panel, true);
            currentPanel = panel;
        }

        public void setPanel(string panel, bool show)
        {
            panels.setPanel(panel, show, false);
        }

        public void goBack()
        {
            GameObject panel = returnCurrentPanel();

            PanelInfo pInfo = panel.GetComponent<PanelInfo>();

            string targetPanel = prevPanel;//just returns last menu game was on

            if (pInfo != null && pInfo.parentPanel != null) // check PanelInfo property, this allows hierachical menus.
            {
                targetPanel = pInfo.parentPanel.name;
            }

            if (targetPanel != null)
            {
                changeMenu(targetPanel, ProjectSettings.Data.menuCancel);
            }

        }

        public void changeMusic(AudioClip m)
        {
            music.Stop();
            if (m == null)
            {
                return;
            }
            music.clip = m;
            music.loop = true;
            music.Play();
        }

        public void OnEnable()
        {
            AudioSource[] audios = GetComponents<AudioSource>();

            audio = audios[0];
            music = audios[1];

            //audio.outputAudioMixerGroup.audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("sfxVol"));
            //music.outputAudioMixerGroup.audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("musicVol"));

            if (HandleMusic) { changeMusic(ProjectSettings.Data.menuMusic); }
            //use an event to change music after pdata is loaded?
            MenuManager.ins = returnInstance();
            panels = gameObject.GetComponent<ShowPanels>();

#if UNITY_ANDROID
        //googlePlay = GameObject.Find("UI").GetComponent<gplayInteractor>();
#endif
            setVolume();

            currentPanel = "MenuPanel";
            if (pausePanel != null)
            {
                pausePanelName = pausePanel.name;
            }
            ProjectData d = ProjectSettings.GetData();
            ProjectSettings.audioPlayer = GetComponent<AudioSource>();
            GameObject f = panels.returnPanel("Fade");
            if (f != null) { fader = f.GetComponent<FaderControl>(); }
            
        }


        public void finishInit()
        {
            //changeMusic(ProjectSettings.data.menuMusic);
        }


        public void setVolume()
        {
            if (PlayerPrefs.GetString("Muted", "no") != "yes")
            {
                AudioListener.volume = 1;
            }
            else
            {
                AudioListener.volume = 0;
            }
        }

        public void processPrefs()
        {
            playSound(ProjectSettings.data.menuConfirm);
            setVolume();
        }

        public void playSound(AudioClip clip)
        {
            audio.Stop();
            if (clip == null)
            { return; }
            audio.PlayOneShot(clip);
        }

        public static void playSound(string sfxName)
        {
            ProjectSettings.Data.PlaySound(sfxName);
        }

        public static void playMusic(AudioClip clip, bool restartMusicIfSame=false)
        {
            if(ins.music.clip != clip || restartMusicIfSame)
            {
                ins.changeMusic(clip);
            }
        }

        public static void statusMessage(string message, float time = 1)
        {
            //GameManager.ins.setUIVal("message", message);
            ins.panels.showPanel("MessagePanel", false);
            ins.msgPanel = ins.panels.returnPanel("MessagePanel");
            ins.Invoke("endStatusMessage", time);
        }

        public void endStatusMessage()
        {
            msgPanel.GetComponent<doAnimAndSleep>().doAnim();
        }

        public void toggleUIelement(GameObject obj)
        {
            obj.SetActive(!obj.activeSelf);
        }

        public static void StartGame(bool runStartGameEvents = true)
        {
            ins.startGame(runStartGameEvents);
        }

        public void startGame(bool runStartGameEvents = true)
        {
            runStartEvent = runStartGameEvents;
            ProjectSettings.Data.PlaySound(ProjectSettings.data.gameStart);
            //error in webgl here
            Time.timeScale = 1;
            panels.hidePanel(currentPanel, true);
            if (fader == null) { doStart(); }
            else { fader.FadeOut(doStart); }
            
            //currentPanel = "GamePanel";

        }

        public void loadGame()
        {
            startGame();
            Invoke("doLoad", 2f);
        }

        public void doLoad()
        {
            continueGameEvent?.Invoke();
        }

        public void doStart()
        {
            if (changeSceneOnStart)
            {
                SceneManager.LoadScene(1, LoadSceneMode.Single);//this causes issues with duplicate menumanagers i believe when you return to menu
            }
            else
            {
                OnLevelWasLoaded(1);
            }

            if(HandleMusic)
            {
                changeMusic(ProjectSettings.data.gameMusic);
            }
            
            MenuManager.gameRunning = true;
            if(runStartEvent)
            {
                startGameEvent?.Invoke();
            }
            fader?.FadeIn(null);
        }

        public void StopGameplay()
        {
            gameRunning = false;
            Time.timeScale = 1;
            if (fader == null) { doStop(); }
            else { fader?.FadeOut(doStop); }
            
        }

        public void doStop()
        {
            stopGameEvent?.Invoke();
            if (changeSceneOnEnd)
            {
                SceneManager.LoadScene(2);
            }
            else
            {
                OnLevelWasLoaded(0);
            }
            changeMenu("MenuPanel");
            
            if(HandleMusic)
            {
                changeMusic(ProjectSettings.data.menuMusic);
            }
            
            fader?.FadeIn(null);
        }

        public void openWeb(string address)
        {
            ProjectSettings.Data.PlaySound(ProjectSettings.data.menuConfirm);
#if !UNITY_WEBGL
            Application.OpenURL(address);
#endif
#if UNITY_WEBGL
        Application.ExternalEval("window.open(\"" + address + "\")");
#endif
        }

        public void Restart()
        {
            UnPause();
            panels.hidePanel("GameOver", true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            MenuManager.ins.changeMusic(ProjectSettings.data.gameMusic);
            startGame();
        }


        public static MenuManager returnInstance()
        {
            MenuManager instance = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuManager>(); // i know this is an extra line but unity freaked out when removed.
            ins = instance;
            return instance;
        }

        public void DoPause(bool showPauseMenu = false)
        {
            MenuManager.isPaused = true;
            //Set time.timescale to 0, this will cause animations and physics to stop updating
            Time.timeScale = 0;
            if (showPauseMenu)
            {
                panels.showPanel(pausePanelName);
            }
        }

        public void doGameOver()
        {
            panels.showPanel("GameOver");
        }

        public void UnPause()
        {
            MenuManager.isPaused = false;
            //Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
            Time.timeScale = 1;
            panels.hidePanel(pausePanelName);
        }

    }
}