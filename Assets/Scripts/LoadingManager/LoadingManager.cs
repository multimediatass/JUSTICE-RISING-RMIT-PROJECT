using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
using TMPro;
using System;

[System.Serializable]
public struct LoadingContent
{
    public Sprite sprite;
    public string message;
}

namespace Tproject
{
    public class LoadingManager : MonoBehaviour
    {
        public static LoadingManager Instance;
        public GameObject loadingPanel;
        public Image loadingBar;
        public Image backgroundImage;
        public TextMeshProUGUI messageText;
        public LoadingContent[] loadingContents;
        public float loadingDuration = 3f;

        [Space]
        public UnityEvent onPanelActive;
        public UnityEvent onPanelClose;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void AddSceneAdditive(string sceneName)
        {
            loadingBar.fillAmount = 0f;
            SelectRandomLoadingContent(); // Select new content before showing the panel
            loadingPanel.SetActive(true);

            object[] arg = new object[2] { sceneName, LoadSceneMode.Additive };

            LeanTween.alphaCanvas(loadingPanel.GetComponent<CanvasGroup>(), 1f, 0.5f)
                     .setOnComplete(() => StartCoroutine(LoadSceneAsync(arg)));
        }

        public void StartLoadScene(string sceneName)
        {
            loadingBar.fillAmount = 0f;
            SelectRandomLoadingContent(); // Select new content before showing the panel
            loadingPanel.SetActive(true);

            object[] arg = new object[2] { sceneName, LoadSceneMode.Single };

            LeanTween.alphaCanvas(loadingPanel.GetComponent<CanvasGroup>(), 1f, 0.5f)
                     .setOnComplete(() => StartCoroutine(LoadSceneAsync(arg)));
        }

        public void StartLoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            loadingBar.fillAmount = 0f;
            SelectRandomLoadingContent(); // Select new content before showing the panel
            loadingPanel.SetActive(true);

            object[] arg = new object[2] { sceneName, mode };

            LeanTween.alphaCanvas(loadingPanel.GetComponent<CanvasGroup>(), 1f, 0.5f)
                     .setOnComplete(() => StartCoroutine(LoadSceneAsync(arg)));
        }

        public void ShowLoadingScreen(Action _funct, float time, Action _fuctOnLoading = null)
        {
            loadingBar.fillAmount = 0f;
            SelectRandomLoadingContent(); // Select new content before showing the panel
            loadingPanel.SetActive(true);

            object[] arg = new object[3] { _funct, time, _fuctOnLoading };

            LeanTween.alphaCanvas(loadingPanel.GetComponent<CanvasGroup>(), 1f, 0.5f)
                .setOnComplete(() => StartCoroutine(DisplayLoadingPanel(arg)));
        }

        private IEnumerator LoadSceneAsync(object[] parms)
        {
            LoadSceneMode lsm = (LoadSceneMode)parms[1];

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(parms[0].ToString(), lsm);
            asyncLoad.allowSceneActivation = false;

            onPanelActive.Invoke();

            float timer = 0;
            while (timer < loadingDuration)
            {
                timer += Time.deltaTime;
                float fillValue = Mathf.Clamp01(timer / loadingDuration);
                loadingBar.fillAmount = fillValue;

                if (fillValue == 1f)
                {
                    asyncLoad.allowSceneActivation = true;
                    LeanTween.alphaCanvas(loadingPanel.GetComponent<CanvasGroup>(), 0f, 0.5f)
                        .setOnComplete(() => Debug.Log("has been changed"));

                    onPanelClose.Invoke();
                }

                yield return null;
            }
        }

        private IEnumerator DisplayLoadingPanel(object[] parms)
        {
            onPanelActive.Invoke();

            Action onFinishFunction = (Action)parms[0];
            float time = (float)parms[1];
            Action onLoadingFunction = (Action)parms[2];

            loadingBar.fillAmount = 0;
            float timer = 0;
            onLoadingFunction?.Invoke();
            while (timer < time)
            {
                timer += Time.deltaTime;
                loadingBar.fillAmount = timer / time;
                yield return null;
            }

            onPanelClose.Invoke();
            onFinishFunction?.Invoke();

            LeanTween.alphaCanvas(loadingPanel.GetComponent<CanvasGroup>(), 0f, 0.4f)
                .setOnComplete(() =>
                {
                    loadingPanel.SetActive(false);
                });
        }

        private void SelectRandomLoadingContent()
        {
            if (loadingContents.Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, loadingContents.Length);
                LoadingContent selectedContent = loadingContents[randomIndex];
                backgroundImage.sprite = selectedContent.sprite;
                messageText.text = selectedContent.message;
            }
        }
    }
}
