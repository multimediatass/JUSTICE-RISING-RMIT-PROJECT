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
            Pause, Play, VisualNovel, GameMenu
        }

        public GameState CurrentGameState;

        public UnityEvent GamemenuState;
        public UnityEvent GamepauseState;
        public UnityEvent GameplayState;

        private void Start()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
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
                    CursorMode(false);
                    break;
            }

            CurrentGameState = state;
        }


        public void CursorMode(bool state)
        {
            Cursor.visible = state;
            Cursor.lockState = CursorLockMode.Locked;
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
