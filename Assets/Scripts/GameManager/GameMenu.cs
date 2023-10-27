using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JusticeRising.GameData;

namespace JusticeRising
{
    public class GameMenu : MonoBehaviour
    {
        public string genderSelected;

        private void Start()
        {
            LoadingManager.instance.CloseLoadingPanel();
        }

        public void SelectGenderType(string type)
        {
            genderSelected = type;
        }

        public void SelectCharacter()
        {
            GameManager.instance.currentPlayerData.playerName = "Taufiq";
            GameManager.instance.currentPlayerData.selectedCharacter = genderSelected;

            LoadingManager.instance.ChangeScene("GamePlay");
        }

    }

}