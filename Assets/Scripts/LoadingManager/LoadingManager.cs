using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace JusticeRising
{
    public class LoadingManager : MonoBehaviour
    {
        public static LoadingManager instance;
        public CanvasGroup bg;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void ChangeScene(string sceneName)
        {
            StartLoading();

            SceneManager.LoadSceneAsync(sceneName);
        }

        public void StartLoading()
        {
            bg.gameObject.SetActive(true);
            bg.alpha = 0;
            bg.LeanAlpha(1, 0.1f);

            Debug.Log($"Start Loading pannel");
        }

        public void CloseLoadingPanel()
        {
            bg.LeanAlpha(1, 0.5f);
            bg.gameObject.SetActive(false);
        }
    }
}
