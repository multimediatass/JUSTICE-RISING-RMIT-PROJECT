using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Tproject;

namespace JusticeRising
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;
        public enum GameState
        {
            Pause, Play, VisualNovel, CutScene, UIInteraction, MainMaps, GameMenu, RestartGamePlay
        }

        public GameManager gameManager;

        public GameState CurrentGameState;

        public UnityEvent GameMenuState;
        public UnityEvent GamepauseState;
        public UnityEvent GameplayState;
        public UnityEvent VisualNovelState;
        public UnityEvent UIInteractionState;
        public UnityEvent MainMapsState;
        public UnityEvent RestartGamePlaysState;

        [Header("TIME MANAGER")]
        public bool isPlayingTime = false;
        public TextMeshProUGUI timeText;
        private float elapsedTime = 0f;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            RestartTime();
        }

        private void Start()
        {
            gameManager = GameManager.instance;

            ResetGameMasterData();

            // UIManager.instance.ShowTutorial(() => ChangeGameState(GameState.Play));
        }

        void Update()
        {
            if (isPlayingTime)
            {
                elapsedTime += Time.deltaTime;
                DisplayTime(elapsedTime);
            }

            if (InputManager.instance.inputAction.PlayerControls.Maps.triggered &&
                CurrentGameState != GameState.MainMaps) ChangeGameState(GameState.MainMaps);
        }

        public void GameTimeState(bool state) => isPlayingTime = state;

        void DisplayTime(float timeToDisplay)
        {
            timeToDisplay += 1;

            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        public void RestartTime()
        {
            elapsedTime = 0f;
        }

        public string GetCurrentTimeString()
        {
            float minutes = Mathf.FloorToInt(elapsedTime / 60);
            float seconds = Mathf.FloorToInt(elapsedTime % 60);
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        public float GetCurrentTimeFloat()
        {
            return elapsedTime;
        }

        public void ChangeGameState(GameState state)
        {
            switch (state)
            {
                case GameState.GameMenu:
                    GameMenuState.Invoke();
                    break;
                case GameState.Pause:
                    GamepauseState.Invoke();
                    break;
                case GameState.Play:
                    GameplayState.Invoke();
                    break;
                case GameState.VisualNovel:
                    VisualNovelState.Invoke();
                    break;
                case GameState.MainMaps:
                    MainMapsState.Invoke();
                    break;
                case GameState.UIInteraction:
                    UIInteractionState.Invoke();
                    break;
                case GameState.RestartGamePlay:
                    RestartGamePlaysState.Invoke();
                    break;
            }

            CurrentGameState = state;

            Debug.Log($"CurrentGameState: {CurrentGameState}");
        }


        public void CursorMode(bool state)
        {
            Cursor.visible = state;

            if (state == false)
                Cursor.lockState = CursorLockMode.Locked;
            else Cursor.lockState = CursorLockMode.None;
        }

        public void PlayGame()
        {
            ChangeGameState(GameState.Play);
        }

        public void PauseGame()
        {
            ChangeGameState(GameState.Pause);
        }

        public void OpenGameMenu()
        {
            ChangeGameState(GameState.GameMenu);
            ResetGameMasterData();
        }

        public void UIInteraction()
        {
            ChangeGameState(GameState.UIInteraction);
        }

        public void ResetGameMasterData()
        {
            RestartTime();
            gameManager.currentPlayerData.ResetData();
        }

        public void RestartGamePlay()
        {
            ChangeGameState(GameState.RestartGamePlay);

            LoadingManager.Instance.ShowLoadingScreen(PlayGame, 3f);
        }

        // public void Reset
    }
}
