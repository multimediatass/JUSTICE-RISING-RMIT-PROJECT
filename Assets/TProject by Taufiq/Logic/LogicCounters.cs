using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Tproject
{
    public class LogicCounters : MonoBehaviour
    {
        [SerializeField] private int currentScore;
        [SerializeField] private int scoreMax;
        private bool isScoreMaxReached = false;

        [Space]
        public UnityEvent OnScoreFinished;
        [Space]

        public TextMeshProUGUI scoreText;

        void Start()
        {
            currentScore = 0;
            UpdateScoreUI();
        }

        public void Increase(int amount)
        {
            if (isScoreMaxReached)
            {
                Debug.Log("Score max reached, cannot add more score.");
                return;
            }

            currentScore += amount;
            UpdateScoreUI();

            // CheckScore();
        }

        public void Decrease(int amount)
        {
            if (isScoreMaxReached)
            {
                Debug.Log("Score max reached, cannot subtract score.");
                return;
            }

            currentScore -= amount;
            if (currentScore < 0)
            {
                currentScore = 0;
            }
            UpdateScoreUI();

            // CheckScore();
        }

        public void CheckCondition()
        {
            if (currentScore >= scoreMax && !isScoreMaxReached)
            {
                isScoreMaxReached = true;
                OnScoreFinished.Invoke();
            }
        }

        public int GetCurrentScore()
        {
            return currentScore;
        }

        private void UpdateScoreUI()
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + currentScore.ToString();
            }
        }
    }
}