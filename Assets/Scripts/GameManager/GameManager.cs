using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private void Start()
        {

        }
    }
}