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
            PlayerData pd = new PlayerData();
            pd.playerName = "Taufiq";
            pd.selectedCharacter = genderSelected;

            GameManager.instance.currentPlayerData = pd;

            LoadingManager.instance.ChangeScene("GamePlay");
        }

    }

}