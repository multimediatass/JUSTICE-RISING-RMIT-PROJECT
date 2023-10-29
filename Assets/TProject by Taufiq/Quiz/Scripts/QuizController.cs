using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using JusticeRising.GameData;

namespace Tproject.Quiz
{
    public class QuizController : MonoBehaviour
    {
        public NpcCard npcCard;
        public List<MQuizContent> contensStaging;
        [System.Serializable]
        public struct MQuizContent
        {
            public string Question;
            public Sprite illustrationSprite;
            public List<string> Option;
            public string CorrectAnswer;

            public MQuizContent Clone()
            {
                MQuizContent copy = new MQuizContent();
                copy.Question = this.Question;
                copy.illustrationSprite = this.illustrationSprite;
                copy.Option = new List<string>(this.Option);
                copy.CorrectAnswer = this.CorrectAnswer;
                return copy;
            }
        }

        [Header("Score UI")]
        public GameObject panelScore;
        public TextMeshProUGUI trueResultText;
        public TextMeshProUGUI falseResultText;
        private int scoreTrue;
        private int scoreFalse;


        [Header("Quiz UI")]
        public GameObject QuizPanel;
        public GameObject validationPanel;
        public Image imgValidationProgress;
        public TextMeshProUGUI questionUI;
        public Image ilustrationImage;
        public List<GameObject> optionBtn;

        int _currentIndex = 0;

        // Handler action
        Action quizFinishToHandler;

        public bool StartQuiz(NpcCard card, Action act)
        {
            npcCard = card;

            if (_currentIndex == card.QuizContents.Count)
            {
                Debug.Log($"Player has been answerd this question section");
                return false;
            }

            quizFinishToHandler = act;

            contensStaging = card.QuizContents.Clone();

            QuizPanel.SetActive(true);
            SetUpQuestionAndAnswers(_currentIndex);

            return true;
        }

        private void SetUpQuestionAndAnswers(int targetQuestion)
        {
            questionUI.text = contensStaging[targetQuestion].Question;

            if (contensStaging[targetQuestion].illustrationSprite != null)
            {
                ilustrationImage.gameObject.SetActive(true);
                ilustrationImage.sprite = contensStaging[targetQuestion].illustrationSprite;
            }
            else ilustrationImage.gameObject.SetActive(false);

            for (int i = 0; i < optionBtn.Count; i++)
            {
                optionBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = contensStaging[targetQuestion].Option[i];
                optionBtn[i].name = contensStaging[targetQuestion].Option[i];
            }
        }

        public void Answer(Button btnAnswerd)
        {
            StartCoroutine(nameof(CheckingAnswer), btnAnswerd);
        }

        IEnumerator CheckingAnswer(Button btnAnswerd)
        {
            validationPanel.SetActive(true);

            bool isChecking = false;

            NpcCard.PlayerAnswerData playerAnswerData = new NpcCard.PlayerAnswerData(contensStaging[_currentIndex].Question, btnAnswerd.name);
            npcCard.QuizAnswerd.Add(playerAnswerData);

            float i = 0;
            while (i < 1f)
            {
                i += Mathf.Clamp01(1f * Time.deltaTime);
                imgValidationProgress.fillAmount = i;

                if (!isChecking)
                {
                    if (btnAnswerd.name == contensStaging[_currentIndex].CorrectAnswer)
                    {
                        scoreTrue++;
                        Debug.Log($"the answer is: true | current score {scoreTrue} / {scoreFalse}");
                    }
                    else
                    {
                        scoreFalse++;
                        Debug.Log($"the answer is: false | current score {scoreTrue} / {scoreFalse}");
                    }

                    isChecking = true;
                }

                yield return null;
            }

            validationPanel.SetActive(false);

            if (_currentIndex < contensStaging.Count) _currentIndex++;

            if (_currentIndex < contensStaging.Count)
            {
                SetUpQuestionAndAnswers(_currentIndex);
            }
            else
            {
                QuizPanel.SetActive(false);
                contensStaging.Clear();
                quizFinishToHandler?.Invoke();

                StartCoroutine(nameof(ShowScore));
                _currentIndex = 0;

                npcCard = null;
            }
        }

        IEnumerator ShowScore()
        {
            panelScore.SetActive(true);
            trueResultText.text = scoreTrue.ToString();
            falseResultText.text = scoreFalse.ToString();

            yield return new WaitForSeconds(3f);

            panelScore.SetActive(false);
        }
    }

    public static class ExtensionMethods
    {
        public static List<QuizController.MQuizContent> Clone(this List<QuizController.MQuizContent> listToClone)
        {
            List<QuizController.MQuizContent> clonedList = new List<QuizController.MQuizContent>();
            foreach (var item in listToClone)
            {
                clonedList.Add(item.Clone());
            }
            return clonedList;
        }
    }
}