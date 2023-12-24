using Tproject;
using UnityEngine;
using UnityEngine.Events;
using Tproject.CutScene;

namespace JusticeRising
{
    public class GameMenu : MonoBehaviour
    {
        public string genderSelected;
        public PlayerCharacter character;

        public UnityEvent OnPlayerSelectedCharacter;

        public CutSceneController cutSceneEnding;

        [Space]
        [Header("UI Components")]
        public GameObject panelMenu;
        public GameObject panelChooseCharacters;

        public void OnClickStartGame()
        {
            panelMenu.SetActive(false);
            panelChooseCharacters.SetActive(true);
        }

        public void SelectGenderType(string type)
        {
            genderSelected = type;
        }

        public void SelectCharacter()
        {
            GameManager.instance.currentPlayerData.characterSelected = genderSelected;
            character.SetUpCharacter(genderSelected);
            cutSceneEnding.SetUpPlayerSpeaker(GetGenderType(genderSelected));

            LoadingManager.Instance.ShowLoadingScreen(() => OnPlayerSelectedCharacter?.Invoke(), 3f);
        }

        private int GetGenderType(string type)
        {
            string a = type;

            return a switch
            {
                "Male" => 0,
                "Female" => 1,
                "Nonbinary" => 2,
                _ => -1
            };
        }
    }

}