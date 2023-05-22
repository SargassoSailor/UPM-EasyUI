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

        public List<GameObject> visibleInMenu = new List<GameObject>();
        public List<GameObject> visibleInGame = new List<GameObject>();

        [HideInInspector] public string currentPanel = "MenuPanel";
        private string prevPanel;

        public static bool gameRunning = false;
        public static bool dataLoaded { get; } = false;

        public UnityEvent startGameEvent;
        public UnityEvent continueGameEvent;
        public UnityEvent stopGameEvent;
        public UnityEvent gameOverEvent;

        public static bool isPaused = false;

        private GameObject msgPanel;
        private bool runStartEvent = true;

        

        public ProjectData projectData;

        public I_AudioProvider audio;

        public int endSceneIndex = 0;

        private void Awake()
        {
            panels = new ShowPanels(gameObject);
            GameMGR.ev_gameOver += doGameOver;
            //audio.Init(gameObject);
            if (MenuManager.ins != null && MenuManager.ins != this)
            {
                Destroy(gameObject);
            }
            else
            {
                MenuManager.ins = this;
            }
            setActivePanels(visibleInMenu);
            if(GetComponent<DontDestroy>() == null) { gameObject.AddComponent<DontDestroy>(); }
        }

        public GameObject returnCurrentPanel()
        {
            return panels.returnPanel(currentPanel);
        }
        public void quitGame()
        {
            Invoke("exit", 1);
            //audio.PlaySound(projectData.menuCancel);
            panels.hidePanel(currentPanel, true);
            //coroutine that calls after everything is complete.
        }

        public void exit()
        {
            Application.Quit();
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
            //projectData.PlaySound(pressSound);
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

        private void OnValidate()
        {
            if (projectData == null) { projectData = ProjectSettings.Data; }
            if(ProjectSettings.data == null) { ProjectSettings.data = projectData; }
        }

        public void OnEnable()
        {
            MenuManager.ins = this;

#if UNITY_ANDROID
        //googlePlay = GameObject.Find("UI").GetComponent<gplayInteractor>();
#endif
            setVolume();

            ProjectSettings.audioPlayer = GetComponent<AudioSource>();
            if (runStartEvent) { GameMGR.levelReady += finishStart; }
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
            //audio.PlaySound(projectData.menuConfirm);
            setVolume();
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
            //projectData.PlaySound(projectData.gameStart);
            //error in webgl here
            Time.timeScale = 1;
            panels.hidePanel(currentPanel, true);
            doStart();
        }

        public void loadGame()
        {
            Time.timeScale = 1;
            panels.hidePanel(currentPanel, true);
            GameMGR.LoadGame();
        }

        public void doStart()
        {
            setActivePanels(visibleInGame);
            GameMGR.StartGame();
        }

        public void setActivePanels(List<GameObject> panels)
        {
            if(panels.Count == 0) { return; }
            foreach (Transform child in gameObject.transform)
            {
                if (child.parent == transform)
                    child.gameObject.SetActive(false);
            }
            foreach(GameObject panel in panels)
            {
                panel.SetActive(true);
            }
        }

        public void finishStart()
        {
            MenuManager.gameRunning = true;
            if (runStartEvent)
            {
                startGameEvent?.Invoke();
            }
        }

        public void StopGameplay()
        {
            gameRunning = false;
            Time.timeScale = 1;
            doStop();
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
            setActivePanels(visibleInMenu);
            GameMGR.StopGame();
            //}
            changeMenu("MenuPanel");
        }

        public static void openWeb(string address)
        {
            //projectData.PlaySound(projectData.menuConfirm);
#if !UNITY_WEBGL
            Application.OpenURL(address);
#endif
#if UNITY_WEBGL
        Application.ExternalEval("window.open(\"" + address + "\")");
#endif
        }

        public void doGameOver(int player = 0)
        {
            gameOverEvent.Invoke();
            panels.showPanel("GameOver", true);
        }

    }
}