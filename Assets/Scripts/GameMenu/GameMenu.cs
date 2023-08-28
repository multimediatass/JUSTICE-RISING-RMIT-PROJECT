using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeRising
{
    public class GameMenu : MonoBehaviour
    {
        public string genderSelected;

        public void SelectGenderType(string type)
        {
            genderSelected = type;
        }

        public void SelectCharacter()
        {
            PlayerDataReference pd = new PlayerDataReference();
            pd.playerName = "Taufiq";
            pd.selectedCharacter = genderSelected;

            GameManager.instance.currentPlayerData.dataMap = pd;

            LoadingManager.instance.ChangeScene("GamePlay");
        }

    }

}