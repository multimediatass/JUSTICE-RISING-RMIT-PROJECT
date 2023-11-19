using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Events;
using JusticeRising;

namespace Tproject.CutScene
{
    using Tproject.AudioManager;

    public class CutSceneController : MonoBehaviour
    {
        private int sectionIndex = 0;
        public List<ContentSection> contentSections;
        public AudioClip backsoundCutScene;

        [Serializable]
        public struct ContentSection
        {
            public string title;
            [TextArea]
            public string contentText;
            public Sprite background;
            public AudioClip sfx;
        }

        public bool isPlayOnStart;
        private State _state = State.COMPLETED;
        private enum State
        {
            PLAYING, SPEEDED_UP, COMPLETED
        }
        private Coroutine _myCoroutine = null;
        private float _speedFactor = 1f;

        [Header("UI Components")]
        public Image UI_background;
        public TextMeshProUGUI UI_titleText;
        public TextMeshProUGUI UI_contentText;
        public GameObject GO_boxPanel;
        public Button Btn_Next;
        public GameObject GO_blackPanel;


        public UnityEvent OnCutSceneStart;
        public UnityEvent AfterCutSceneClosed;


        private void Start()
        {
            sectionIndex = 0;

            if (isPlayOnStart)
                StartCutScene();

            Btn_Next.onClick.AddListener(OnClickBtnAction);
        }

        public void StartCutScene()
        {
            OnCutSceneStart?.Invoke();

            GO_blackPanel.SetActive(true);
            LeanTween.alpha(GO_blackPanel.GetComponent<RectTransform>(), 1f, .5f).setOnComplete(RequestTypeText);
            GO_boxPanel.SetActive(true);


            if (backsoundCutScene != null) AudioManager.Instance.PlayBacksound(backsoundCutScene);
        }

        private void RequestTypeText()
        {
            UI_background.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            LeanTween.alpha(GO_blackPanel.GetComponent<RectTransform>(), 0f, .5f).setOnComplete(() => GO_blackPanel.SetActive(false));
            Btn_Next.gameObject.SetActive(true);

            ContentSection scriptReady = contentSections[sectionIndex];

            if (scriptReady.background != null)
            {
                UI_background.gameObject.SetActive(true);
                LeanTween.scale(GO_boxPanel, new Vector3(1f, 1f, 1f), 1f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic);
                _myCoroutine = StartCoroutine(TypeText(scriptReady.title, scriptReady.contentText, scriptReady.sfx, scriptReady.background));

                LeanTween.scale(UI_background.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 20f);
            }
            else
            {
                UI_background.gameObject.SetActive(false);
                LeanTween.scale(GO_boxPanel, new Vector3(1f, 1f, 1f), 1f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic);
                _myCoroutine = StartCoroutine(TypeText(scriptReady.title, scriptReady.contentText, scriptReady.sfx));
            }


        }

        private IEnumerator TypeText(string title, string contentScript, AudioClip clip = null, Sprite bg = null)
        {
            UI_titleText.text = title;
            UI_contentText.text = "";
            _state = State.PLAYING;
            // int wordIndex = 0;

            Btn_Next.GetComponentInChildren<TextMeshProUGUI>().text = "Speed Up";

            if (clip != null) AudioManager.Instance.PlaySFX(clip);
            if (bg != null) UI_background.sprite = bg;

            while (_state != State.COMPLETED)
            {
                // UI_contentText.text += contentScript[wordIndex];
                // yield return new WaitForSeconds(_speedFactor * 0.05f);
                // if (++wordIndex == contentScript.Length)
                // {
                //     Btn_Next.GetComponentInChildren<TextMeshProUGUI>().text = sectionIndex < contentSections.Count - 1 ? "Next" : "Close";

                //     _speedFactor = 1f;

                //     LeanTween.cancel(UI_background.gameObject);

                //     _state = State.COMPLETED;
                //     _myCoroutine = null;
                //     break;
                // }

                UI_contentText.text = contentScript;
                yield return new WaitForSeconds(0.05f);

                if (UI_contentText.text.Length == contentScript.Length)
                {
                    Btn_Next.GetComponentInChildren<TextMeshProUGUI>().text = sectionIndex < contentSections.Count - 1 ? "Next" : "Close";

                    // LeanTween.cancel(UI_background.gameObject);

                    _state = State.COMPLETED;
                    _myCoroutine = null;
                    break;
                }
            }
        }

        private bool isPlayNormalSpeed() => _speedFactor == 1f && _state == State.PLAYING;

        public void OnClickBtnAction()
        {
            if (_state == State.COMPLETED)
            {
                if (sectionIndex < contentSections.Count - 1)
                {
                    sectionIndex++;

                    GO_blackPanel.SetActive(true);
                    LeanTween.cancel(UI_background.gameObject);
                    LeanTween.alpha(GO_blackPanel.GetComponent<RectTransform>(), 1f, .5f).setOnComplete(RequestTypeText);
                }
                else
                {
                    AudioManager.Instance.DeleteBacksound();
                    AudioManager.Instance.StopSFX();
                    AudioManager.Instance.PlayDefaultBacksound();

                    GO_blackPanel.SetActive(true);
                    LeanTween.alpha(GO_blackPanel.GetComponent<RectTransform>(), 1f, .5f).setLoopPingPong(1).setOnStart(() => AfterCutSceneClosed?.Invoke());
                }
            }
            else if (isPlayNormalSpeed())
            {
                _state = State.SPEEDED_UP;
                _speedFactor = 0.001f;

                Btn_Next.GetComponentInChildren<TextMeshProUGUI>().text = "...";
            }
        }

        public void BackToGameMenu()
        {
            // LoadingManager.instance.ChangeScene("GameMenu");
        }
    }
}

