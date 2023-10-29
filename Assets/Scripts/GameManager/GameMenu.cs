
using UnityEngine;
using JusticeRising.GameData;

namespace JusticeRising
{
    public class GameMenu : MonoBehaviour
    {
        public PlayerData playerData;
        public string genderSelected;

        [Space]
        [Header("UI Components")]
        public GameObject panelMenu;
        public GameObject panelChooseCharacters;

        private void Start()
        {
            // LoadingManager.instance.CloseLoadingPanel();
        }

        public void OnClickStartGame()
        {
            if (!string.IsNullOrEmpty(playerData.selectedCharacter))
            {
                LoadingManager.instance.ChangeScene("GamePlay");
            }
            else
            {
                panelMenu.SetActive(false);
                panelChooseCharacters.SetActive(true);
            }
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