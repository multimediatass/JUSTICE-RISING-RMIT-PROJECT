using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Tproject.Tools;
using JusticeRising.GameData;
using System.Linq;

namespace Tproject.VisualNovelV2
{
    // Creator Instagram: @shantaufiq

    using Tproject.AudioManager;

    public class DialogsController : MonoBehaviour
    {
        public static DialogsController instance;

        public NpcCard npcCard;
        private List<string> _conversationSelected = new List<string>();

        [Header("Dialog Scripts Manager")]
        public DialogScript DialogScripts;

        [System.Serializable]
        public struct DialogScript
        {
            public int PlayerOpportunity;
            public string npcName;
            public List<section> sectionList;

            [System.Serializable]
            public struct section
            {
                [TextArea]
                public string Question;
                [TextArea]
                public string Answer;
                public AudioClip SFX;
                public Sprite npcImage;
            }

            public DialogScript Clone()
            {
                DialogScript copy = new DialogScript();
                copy.PlayerOpportunity = this.PlayerOpportunity;
                copy.npcName = this.npcName;
                copy.sectionList = new List<section>(this.sectionList.Select(s => new section
                {
                    Question = s.Question,
                    Answer = s.Answer,
                    SFX = s.SFX,
                    npcImage = s.npcImage
                }));
                return copy;
            }
        }

        // reset player opertunity
        Action<int> SetUpPlayerOpt;

        bool isPlayerHasOpportunity = false;
        Action playerDontHaveOpportunity;
        Action playerHasOpportunity;

        [Header("UI Components")]
        [SerializeField] GameObject pannelUI;
        [SerializeField] Image speakerImage;
        [SerializeField] Transform contentParent;
        [SerializeField] TextMeshProUGUI plankNpcName;
        [SerializeField] GameObject btnClose;
        [SerializeField] GameObject btnSpeedUp;
        public List<RowItem> rowItems;
        private RowItem _rowQuestion;

        private State _state = State.COMPLETED;
        public enum State
        {
            PLAYING, SPEEDED_UP, COMPLETED
        }
        private Coroutine _myCoroutine = null;
        private float _speedFactor = 1f;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        public void StartDialog(NpcCard card, Action _PlayerHasOpportunity, Action _PlayerDontHaveOpportunity)
        {
            npcCard = card;

            DialogScripts = card.DialogScripts.Clone();
            playerDontHaveOpportunity = _PlayerDontHaveOpportunity;
            playerHasOpportunity = _PlayerHasOpportunity;

            pannelUI.SetActive(true);

            if (CheckingOpt())
                ShowQuestion();
            else
            {
                var item = rowItems.Find((x) => x.rowItemType == RowItem.ItemType.Cleft);
                _rowQuestion = Instantiate(item, contentParent);
                _rowQuestion.textUI.text = $"Sorry I am bussy now!!";
            }

            speakerImage.sprite = card.npcImages[0];

            if (DialogScripts.PlayerOpportunity < 1) isPlayerHasOpportunity = false;
            else isPlayerHasOpportunity = true;
        }
        public void StartDialog(DialogScript scr, Action<int> popt)
        {
            DialogScripts = scr;
            SetUpPlayerOpt = popt;

            pannelUI.SetActive(true);

            if (CheckingOpt())
                ShowQuestion();
            else
            {
                var item = rowItems.Find((x) => x.rowItemType == RowItem.ItemType.Cleft);
                _rowQuestion = Instantiate(item, contentParent);
                _rowQuestion.textUI.text = $"Sorry I am bussy now!!";
            }
        }

        private List<string> GetAllQuestion()
        {
            List<string> temp = new List<string>();
            foreach (var item in DialogScripts.sectionList)
            {
                temp.Add(item.Question);
            }
            return temp;
        }

        private List<string> GetDialogSection(int id)
        {
            List<string> temp = new List<string>();
            var _section = DialogScripts.sectionList[id];
            temp.Add(_section.Question);
            temp.Add(_section.Answer);

            return temp;
        }

        private AudioClip GetSFX(int id)
        {
            AudioClip temp = DialogScripts.sectionList[id].SFX != null ? DialogScripts.sectionList[id].SFX : null;
            return temp;
        }

        private bool CheckingOpt()
        {
            bool result;
            if (DialogScripts.PlayerOpportunity < 1)
            {
                result = false;
                btnClose.SetActive(true);
                btnSpeedUp.SetActive(false);
                // Debug.Log($"isn't enough opportunity: {DialogScripts.PlayerOpportunity}");
            }
            else
            {
                if (npcCard.ConversationSelected.Count > 0)
                {
                    btnSpeedUp.SetActive(true);
                }

                btnClose.SetActive(false);
                result = true;
            }

            return result;
        }

        private void ShowQuestion()
        {
            // set npc name on UI
            plankNpcName.text = DialogScripts.npcName;

            var item = rowItems.Find((x) => x.rowItemType == RowItem.ItemType.Question);
            // Debug.Log($"Got item name : {test.gameObject.name}");

            _rowQuestion = Instantiate(item, contentParent);
            _rowQuestion.textUI.text = $"You have {DialogScripts.PlayerOpportunity} opertunity to ask a question, Choose witch one do you want to ask !";
            foreach (var i in GetAllQuestion())
            {
                _rowQuestion.questionList.Add(i);
            }
            _rowQuestion = null;
        }

        public void ShowDialogSection(int sectionId)
        {
            StartCoroutine(InstanceRowItem(GetDialogSection(sectionId), GetSFX(sectionId)));

            if (DialogScripts.sectionList[sectionId].npcImage)
                speakerImage.sprite = DialogScripts.sectionList[sectionId].npcImage;

            DialogScripts.sectionList.RemoveAt(sectionId);
            DialogScripts.PlayerOpportunity--;

        }

        private IEnumerator InstanceRowItem(List<string> textPrint, AudioClip sfx = null)
        {
            btnSpeedUp.SetActive(true);

            int i = 0;
            while (i < 3)
            {
                var row = rowItems.Find((x) => x.rowItemType == RowItem.ItemType.Line);

                switch (i)
                {
                    case 0:
                        row = rowItems.Find((x) => x.rowItemType == RowItem.ItemType.Cright);
                        break;
                    case 1:
                        row = rowItems.Find((x) => x.rowItemType == RowItem.ItemType.Cleft);
                        break;
                    case 2:
                        row = rowItems.Find((x) => x.rowItemType == RowItem.ItemType.Line);
                        break;
                }

                if (i < 2)
                {
                    var item = Instantiate(row, contentParent);

                    if (i == 1 && sfx != null)
                        _myCoroutine = StartCoroutine(TypeText(textPrint[i], item.textUI, sfx));
                    else _myCoroutine = StartCoroutine(TypeText(textPrint[i], item.textUI));

                    _conversationSelected.Add(textPrint[i]);
                }
                else
                {
                    Instantiate(row, contentParent);
                }

                yield return new WaitUntil(() => _state == State.COMPLETED);

                i++;
            }

            CheckingOpt();
            _speedFactor = 1f;
        }

        private IEnumerator TypeText(string text, TextMeshProUGUI target, AudioClip clip = null)
        {
            target.text = "";
            _state = State.PLAYING;
            // int wordIndex = 0;

            if (clip != null) AudioManager.Instance.PlaySFX(clip);

            while (_state != State.COMPLETED)
            {
                // target.text += text[wordIndex];
                // yield return new WaitForSeconds(_speedFactor * 0.05f);

                // if (++wordIndex == text.Length)
                // {
                //     _state = State.COMPLETED;
                //     _myCoroutine = null;
                //     break;
                // }

                // without running text
                target.text = text;
                yield return new WaitForSeconds(0.05f);

                if (target.text.Length == text.Length)
                {
                    _state = State.COMPLETED;
                    _myCoroutine = null;
                    break;
                }

            }
        }

        private bool isPlayNormalSpeed() => _speedFactor == 1f && _state == State.PLAYING;

        public void OnclickSpeedUpOrNext()
        {
            if (_state == State.COMPLETED && CheckingOpt())
            {
                ShowQuestion();
                AudioManager.Instance.StopSFX();
            }
            else if (isPlayNormalSpeed())
            {
                _state = State.SPEEDED_UP;
                _speedFactor = 0f;
            }

            btnSpeedUp.SetActive(false);
        }

        public void OnclickCloseDialog()
        {
            if (GOMod.RemoveGOChildren(contentParent)) pannelUI.SetActive(false);

            if (isPlayerHasOpportunity) playerHasOpportunity?.Invoke();
            else playerDontHaveOpportunity?.Invoke();

            npcCard.AddConversationSelected(_conversationSelected);
            npcCard.DialogScripts.PlayerOpportunity = DialogScripts.PlayerOpportunity;

            // LevelManager.instance.ChangeGameState(LevelManager.GameState.Play);

            AudioManager.Instance.StopSFX();

            DialogScripts = new DialogScript();
        }
    }
}