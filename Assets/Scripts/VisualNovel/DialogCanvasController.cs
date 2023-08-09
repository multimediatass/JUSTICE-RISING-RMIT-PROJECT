using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace JusticeRising.VisualNovel
{
    using JusticeRising.AudioManager;

    public class DialogCanvasController : MonoBehaviour
    {
        [SerializeField] private DialogsNPC dialogReady; // dialog
        [SerializeField] private List<string> scriptReady; // sentece
        int _sectionId = 0;
        List<string> _stagingSection = new List<string>();

        public enum State
        {
            PLAYING, SPEEDED_UP, COMPLETED
        }

        private State _state = State.COMPLETED;
        private Coroutine _myCoroutine = null;
        private float _speedFactor = 1f;

        [Header("UI Components")]
        public Transform contentParentDialog;
        [SerializeField] RowItem _rowItemRight;
        [SerializeField] RowItem _rowItemLeft;
        public GameObject buttonAction;
        [SerializeField] TextMeshProUGUI speakerName;
        [SerializeField] Image speakerImage;

        public UnityEvent onVisualNovelEnable;
        public UnityEvent onVisualNovelDisable;


        // music & sfx settings
        float defaultMusicVol;

        private void Update()
        {
            MusicVol();
        }

        private void MusicVol()
        {
            if (IsStillWrite() && contentParentDialog.childCount != scriptReady.Count)
            {
                if (defaultMusicVol > 0.4f)
                {
                    AudioManager.Instance.MusicVolume(defaultMusicVol - 0.4f);
                    // Debug.Log($"current volume: {AudioManager.Instance.GetMusicVolume()}");
                }
            }
            else if (!IsStillWrite() && contentParentDialog.childCount == scriptReady.Count)
            {
                AudioManager.Instance.MusicVolume(defaultMusicVol);
            }
        }

        private bool IsStillWrite()
        {
            return _state == State.PLAYING || _state == State.SPEEDED_UP;
        }

        public void OnClickOpenQuestion(DialogsNPC dialog)
        {
            ResetDefault();

            defaultMusicVol = AudioManager.Instance.GetMusicVolume();

            dialogReady = dialog;

            onVisualNovelEnable.Invoke();

            List<string> staging = dialog.GetListDialogScript();

            if (staging.Count > 0 && staging.Count % 2 == 0)
            {
                foreach (var item in staging)
                {
                    scriptReady.Add(item);
                }
            }
            else
            {
                scriptReady.Add("hi .. can you help me");
                scriptReady.Add("sory, i can't answer");

                Debug.LogWarning($"NPC's dialog script is incorrect format. script count: {staging.Count}");
            }

            RemoveChildren(contentParentDialog);
            StartOrNextDialog();
        }

        //! START: NPC Doesn't have DialogsNPC
        public void OnClickOpenQuestion()
        {
            ResetDefault();

            onVisualNovelEnable.Invoke();

            scriptReady.Add("hi .. can you help me");
            scriptReady.Add("sory, i can't answer now :)");

            RemoveChildren(contentParentDialog);
            StartOrNextDialog();
        }
        //! END: NPC Doesn't have DialogsNPC

        private void SetUpPannel()
        {
            speakerName.text = dialogReady.Sentences[_sectionId].nameSpeaker;
            speakerImage.sprite = dialogReady.Sentences[_sectionId].imageSpeaker;
        }

        private void StartOrNextDialog()
        {
            for (int i = _sectionId; i <= scriptReady.Count - 1; i++)
            {
                _stagingSection.Add(scriptReady[i]);
            }

            StartCoroutine(InstanceRowItem());
        }

        private IEnumerator InstanceRowItem()
        {
            yield return new WaitUntil(() => _stagingSection.Count > 1);

            int i = 0;
            while (i < 2)
            {
                RowItem row = _sectionId % 2 == 0 ? _rowItemRight : _rowItemLeft;
                RowItem item = Instantiate(row, contentParentDialog);
                _myCoroutine = StartCoroutine(TypeText(_stagingSection[i], item.tmpUI));

                yield return new WaitUntil(() => _state == State.COMPLETED);

                i++;
            }

            _stagingSection.Clear();
            _speedFactor = 1f;

            if (contentParentDialog.childCount == scriptReady.Count)
            {
                buttonAction.GetComponentInChildren<TextMeshProUGUI>().text = "Close dialog";
                buttonAction.GetComponent<Button>().interactable = true;
            }
            else
            {
                buttonAction.GetComponentInChildren<TextMeshProUGUI>().text = "Ask a question";
                buttonAction.GetComponent<Button>().interactable = true;
            }
        }

        private IEnumerator TypeText(string text, TextMeshProUGUI target)
        {
            SetUpPannel();

            target.text = "";
            _state = State.PLAYING;
            int wordIndex = 0;

            buttonAction.GetComponentInChildren<TextMeshProUGUI>().text = "Skip";

            if (dialogReady.Sentences[_sectionId].SFX != null)
                AudioManager.Instance.PlaySFX(dialogReady.Sentences[_sectionId].SFX);

            while (_state != State.COMPLETED)
            {
                target.text += text[wordIndex];
                yield return new WaitForSeconds(_speedFactor * 0.05f);
                if (++wordIndex == text.Length)
                {
                    _state = State.COMPLETED;
                    _myCoroutine = null;
                    _sectionId += 1;
                    break;
                }
            }
        }

        public void OnClickButtonAction()
        {
            if (_state == State.COMPLETED)
            {
                if (_sectionId < scriptReady.Count - 1)
                {
                    StartOrNextDialog();
                }
                else if (contentParentDialog.childCount == scriptReady.Count)
                {
                    onVisualNovelDisable.Invoke();

                    ResetDefault();
                }
            }
            else if (_state == State.PLAYING)
            {
                _state = State.SPEEDED_UP;
                _speedFactor = 0.001f;

                buttonAction.GetComponent<Button>().interactable = false;
            }
        }

        private void RemoveChildren(Transform parent)
        {
            // Iterasi melalui semua child GameObject
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Transform child = parent.GetChild(i);
                // Hapus child GameObject
                Destroy(child.gameObject);
            }

            // Debug.Log($"Clear {parent.gameObject.name} 's childern");
        }

        private void ResetDefault()
        {
            RemoveChildren(contentParentDialog);
            _stagingSection.Clear();
            _speedFactor = 1f;
            _sectionId = 0;
            scriptReady.Clear();
            dialogReady = null;
            speakerName.text = "...";
            buttonAction.GetComponent<Button>().interactable = true;
        }
    }
}
