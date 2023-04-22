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
        public UnityEvent gameOverEvent;

        public static bool isPaused = false;

        private GameObject msgPanel;
        private bool runStartEvent = true;

        public FaderControl fader;

        public bool HandleMusic = true;

        public ProjectData projectData;

        public int endSceneIndex = 0;

        private void Awake()
        {
            panels = new ShowPanels(gameObject);
            NewGameMGR.ev_gameOver += doGameOver;
        }

        public GameObject returnCurrentPanel()
        {
            return panels.returnPanel(currentPanel);
        }
        public void quitGame()
        {
            Invoke("exit", 1);
            projectData.PlaySound(projectData.menuCancel);
            panels.hidePanel(currentPanel, true);
            //coroutine that calls after everything is complete.
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
            ProjectSettings.audioPlayer = GetComponent<AudioSource>();

            string[] hideInMenu = { "" };
            string[] hideInGame = { "" };
            string[] showInGame = { "" };
            string[] showInMenu = { "" };

            if (projectData.showInMenu != "")
            {
                showInMenu = projectData.showInMenu.Split(',');
            }
            if (projectData.hideInMenu != "")
            {
                hideInMenu = projectData.hideInMenu.Split(',');
            }
            if (projectData.showInGame != "")
            {
                showInGame = projectData.showInGame.Split(',');
            }
            if (projectData.hideInGame != "")
            {
                hideInGame = projectData.hideInGame.Split(',');
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
                pressSound = projectData.menuConfirm;
            }
            projectData.PlaySound(pressSound);
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

        public static void setPanel(string panel, bool show)
        {
            ins.panels.setPanel(panel, show, false);
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
                changeMenu(targetPanel, projectData.menuCancel);
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

        private void OnValidate()
        {
            if (projectData == null) { projectData = ProjectSettings.Data; }
            if(ProjectSettings.data == null) { ProjectSettings.data = projectData; }
        }

        public void OnEnable()
        {
            AudioSource[] audios = GetComponents<AudioSource>();

            audio = audios[0];
            music = audios[1];

            //audio.outputAudioMixerGroup.audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("sfxVol"));
            //music.outputAudioMixerGroup.audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("musicVol"));

            if (HandleMusic) { changeMusic(projectData.menuMusic); }
            //use an event to change music after pdata is loaded?
            MenuManager.ins = returnInstance();

#if UNITY_ANDROID
        //googlePlay = GameObject.Find("UI").GetComponent<gplayInteractor>();
#endif
            setVolume();

            currentPanel = "MenuPanel";
            if (pausePanel != null)
            {
                pausePanelName = pausePanel.name;
            }
            ProjectSettings.audioPlayer = GetComponent<AudioSource>();
            GameObject f = panels.returnPanel("Fade");
            if (f != null) { fader = f.GetComponent<FaderControl>(); }
            
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
            playSound(projectData.menuConfirm);
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
            ProjectSettings.data.PlaySound(sfxName);
        }

        public static void playMusic(AudioClip clip, bool restartMusicIfSame=false)
        {
            if(ins.music.clip != clip || restartMusicIfSame || !ins.music.isPlaying)
            {
                ins.changeMusic(clip);
            }
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
            projectData.PlaySound(projectData.gameStart);
            //error in webgl here
            Time.timeScale = 1;
            panels.hidePanel(currentPanel, true);
            if (fader == null) { doStart(); }
            else { fader.FadeOut(doStart); }
            
            //currentPanel = "GamePanel";

        }

        public void loadGame()
        {
            NewGameMGR.LoadGame();
        }

        public void doStart()
        {
            /*if (changeSceneOnStart)
            {
                SceneManager.sceneLoaded += NewGameMGR.StartGameDelayed;
                SceneManager.LoadScene(1, LoadSceneMode.Single);//this causes issues with duplicate menumanagers i believe when you return to menu
            }
            else
            {*/
                OnLevelWasLoaded(1);
                NewGameMGR.StartGame();
            //}

            if(HandleMusic)
            {
                changeMusic(projectData.gameMusic);
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
            /*if (changeSceneOnEnd)
            {
                SceneManager.LoadScene(endSceneIndex);
            }
            else
            {*/
                OnLevelWasLoaded(0);
                NewGameMGR.StopGame();
            //}
            changeMenu("MenuPanel");
            
            if(HandleMusic)
            {
                changeMusic(projectData.menuMusic);
            }
            
            fader?.FadeIn(null);
        }

        public void openWeb(string address)
        {
            projectData.PlaySound(projectData.menuConfirm);
#if !UNITY_WEBGL
            Application.OpenURL(address);
#endif
#if UNITY_WEBGL
        Application.ExternalEval("window.open(\"" + address + "\")");
#endif
        }

        public static MenuManager returnInstance()
        {
            MenuManager instance = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuManager>(); // i know this is an extra line but unity freaked out when removed.
            ins = instance;
            return instance;
        }

        public void doGameOver(int player = 0)
        {
            gameOverEvent.Invoke();
            panels.showPanel("GameOver", true);
        }

    }
}