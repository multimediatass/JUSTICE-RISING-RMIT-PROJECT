using Tproject;
using UnityEngine;
using UnityEngine.Events;

namespace JusticeRising
{
    public class GameMenu : MonoBehaviour
    {
        public string genderSelected;
        public PlayerCharacter character;

        public UnityEvent OnPlayerSelectedCharacter;

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

            LoadingManager.Instance.ShowLoadingScreen(() => OnPlayerSelectedCharacter?.Invoke(), 3f);
        }
    }

}