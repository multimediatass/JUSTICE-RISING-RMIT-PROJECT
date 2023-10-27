using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JusticeRising.GameData;

namespace JusticeRising
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public PlayerData currentPlayerData;

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

        public void AddNpcCard(NpcCard newCard)
        {
            currentPlayerData.npcResumeActivity.Add(newCard);

            Debug.Log(newCard.npcName);
        }
    }
}