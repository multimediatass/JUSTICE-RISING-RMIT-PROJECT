using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JusticeRising
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;
        public enum GameState
        {
            Pause, Play, VisualNovel, GameMenu, CutScene
        }

        public GameState CurrentGameState;

        public UnityEvent GamemenuState;
        public UnityEvent GamepauseState;
        public UnityEvent GameplayState;
        public UnityEvent VisualNovelState;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            ChangeGameState(GameState.Play);
        }

        public void ChangeGameState(GameState state)
        {
            switch (state)
            {
                case GameState.GameMenu:
                    GamemenuState.Invoke();
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
            GameplayState?.Invoke();
        }

        public void PauseGame()
        {
            CursorMode(true);
        }
    }
}
