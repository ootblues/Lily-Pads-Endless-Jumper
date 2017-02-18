using UnityEngine;
using System.Collections;

namespace LilyPadsEndlessJumper
{
    public class GoBack : MonoBehaviour
    {
        public enum BackTarget
        {
            NoAction,
            DialogDismiss,
            PostGameMenu,
            MainMenu,
            Previous,
            QuitGame,
        }

        [SerializeField]
        GameObject m_PauseMenuGameObject = null;

        [SerializeField]
        CanvasGroup[] m_PausedHideUI = null;

        //public delegate void BackButtonPressed(BackTarget backTarget);
        //public static event BackButtonPressed OnBackButtonPressed;

        void Awake()
        {
            if(m_PauseMenuGameObject)
            {
                m_PauseMenuGameObject.SetActive(false);
            }
        }

        void Update()
        {
            //if (GameManager.Instance.IsLoading()) return;
            BackButton();
        }

        public void ReloadLevel()
        {
            Time.timeScale = 1.0f;
            Application.LoadLevel(Application.loadedLevel);
        }

        public void PauseGame()
        {
            if(m_PauseMenuGameObject)
            {
                m_PauseMenuGameObject.SetActive(true);
            }
            Time.timeScale = 0.0f;
            for (int i = 0; i < m_PausedHideUI.Length; i++)
            {
                if (m_PausedHideUI[i])
                {
                    m_PausedHideUI[i].alpha = 0.2f;
                }
            }
        }

        public void ResumeGame()
        {
            if (m_PauseMenuGameObject)
            {
                m_PauseMenuGameObject.SetActive(false);
            }
            Time.timeScale = 1.0f;
            for (int i = 0; i < m_PausedHideUI.Length; i++)
            {
                if (m_PausedHideUI[i])
                {
                    m_PausedHideUI[i].alpha = 1.0f;
                }
            }
        }

        public void TogglePause()
        {
            if (Time.timeScale == 0.0f)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        void BackButton()
        {
            if (Application.platform == RuntimePlatform.Android
                || Application.platform == RuntimePlatform.WindowsEditor
                || Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.WindowsWebPlayer
                || Application.platform == RuntimePlatform.OSXWebPlayer)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    //var backTarget = BackTarget.NoAction;

                    /*var dialogObjects = GameObject.FindGameObjectsWithTag(Tags.DIALOG)
                        .OrderBy(x => x.GetComponent<GraphicRaycaster>().renderOrderPriority);
                    foreach (var bo in dialogObjects)
                    {
                        if (bo.activeSelf)
                        {
                            var baseContainer = bo.GetComponentInChildren<BaseContainer>();
                            if (baseContainer is LoadingScreenContainer)
                            {
                                return;
                            }
                            if (baseContainer is QuitGameContainer ||
                                baseContainer is EndRoundContainer ||
                                baseContainer is CameraControlsContainer)
                            {
                                baseContainer.DeclineClick();
                                return;
                            }

                            var dialogCanvas = bo.GetComponent<ModalDialogCanvas>();
                            if (dialogCanvas is ModalDialogCanvas)
                            {
                                dialogCanvas.DeclineClick();
                                return;
                            }

                            var sender = baseContainer.sender;
                            bo.SetActive(false);
                            sender.SetActive(true);
                            GameObject.Destroy(bo);
                            backTarget = BackTarget.DialogDismiss;
                            if (OnBackButtonPressed != null)
                            {
                                OnBackButtonPressed(backTarget);
                            }
                            return;
                        }
                    }*/
                    /*if (Application.loadedLevelName == GameManager.MAIN_MENU)
                    {
                        backTarget = BackTarget.QuitGame;
                    }
                    else if (Application.loadedLevelName == GameManager.PRE_MENU ||
                        Application.loadedLevelName == GameManager.POST_MENU)
                    {
                        backTarget = BackTarget.MainMenu;
                    }
                    else if (Application.loadedLevelName == GameManager.OPTIONS_MENU)
                    {
                        backTarget = BackTarget.Previous;
                    }
                    else if (Application.loadedLevelName == GameManager.Instance.currentLevelName)
                    {
                        backTarget = BackTarget.PostGameMenu;
                    }
                    else
                    {
                        Location lastLocation = GameManager.Instance.lastLocation;
                        if (!string.IsNullOrEmpty(lastLocation.sceneName))
                        {
                            backTarget = BackTarget.Previous;
                        }
                        else
                        {
                            backTarget = BackTarget.MainMenu;
                        }
                    }

                    if (OnBackButtonPressed != null)
                    {
                        OnBackButtonPressed(backTarget);
                    }

                    switch (backTarget)
                    {
                        case BackTarget.NoAction:
                            break;
                        case BackTarget.DialogDismiss:
                            break;
                        case BackTarget.MainMenu:
                            GameManager.Instance.LoadLevel(0);
                            break;
                        case BackTarget.PostGameMenu:
                            GameManager.Instance.PromptToEndRound();
                            break;
                        case BackTarget.Previous:
                            Location lastLocation = GameManager.Instance.lastLocation;
                            GameManager.Instance.LoadLevel(lastLocation.sceneName);
                            break;
                        case BackTarget.QuitGame:
                            GameManager.Instance.PromptToQuitGame();
                            break;
                    }*/
                    if (Time.timeScale == 1.0f)
                    {
                        PauseGame();
                    }
                    else
                    {
                        QuitGame();
                    }
                }
            }
        }

        public void QuitGame()
        {
            //Quit per platform
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        string webplayerQuitURL = "http://callowcreation.com";
        Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif
        }
    }
}