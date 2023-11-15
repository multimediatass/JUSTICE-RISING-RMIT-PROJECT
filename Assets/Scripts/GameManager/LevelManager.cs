using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace JusticeRising
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;
        public enum GameState
        {
            Pause, Play, VisualNovel, CutScene, UIInteraction, MainMaps
        }

        public GameState CurrentGameState;

        public UnityEvent GamepauseState;
        public UnityEvent GameplayState;
        public UnityEvent VisualNovelState;
        public UnityEvent UIInteractionState;
        public UnityEvent MainMapsState;

        [Header("TIME MANAGER")]
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
            // ChangeGameState(GameState.Play);

            // UIManager.instance.ShowTutorial(() => ChangeGameState(GameState.Play));
        }

        void Update()
        {
            elapsedTime += Time.deltaTime;
            DisplayTime(elapsedTime);

            if (InputManager.instance.inputAction.PlayerControls.Maps.triggered &&
                CurrentGameState != GameState.MainMaps) ChangeGameState(GameState.MainMaps);
        }

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

        public string GetCurrentTime()
        {
            float minutes = Mathf.FloorToInt(elapsedTime / 60);
            float seconds = Mathf.FloorToInt(elapsedTime % 60);
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        public void ChangeGameState(GameState state)
        {
            switch (state)
            {
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
            }

            CurrentGameState = state;
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

        public void UIInteraction()
        {
            ChangeGameState(GameState.UIInteraction);
        }
    }
}
