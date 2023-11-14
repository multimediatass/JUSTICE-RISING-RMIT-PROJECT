
using UnityEngine;
using JusticeRising.GameData;
using Tproject.Authentication;

namespace JusticeRising
{
    public class GameMenu : MonoBehaviour
    {
        public PlayerData playerData;
        public string genderSelected;
        public AuthenticationManager auth;

        [Space]
        [Header("UI Components")]
        public GameObject panelMenu;
        public GameObject panelChooseCharacters;
        public GameObject panelLogin;

        private void Awake()
        {
            if (auth.CheckLoginState())
                panelLogin.SetActive(false);
            else
                panelLogin.SetActive(true);
        }

        private void Start()
        {
            LoadingManager.instance.CloseLoadingPanel();
        }

        public void OnClickStartGame()
        {
            // if (!string.IsNullOrEmpty(playerData.characterSelected))
            // {
            //     LoadingManager.instance.ChangeScene("GamePlay");
            // }
            // else
            // {
            panelMenu.SetActive(false);
            panelChooseCharacters.SetActive(true);
            // }
        }

        public void SelectGenderType(string type)
        {
            genderSelected = type;
        }

        public void SelectCharacter()
        {
            GameManager.instance.currentPlayerData.characterSelected = genderSelected;

            LoadingManager.instance.ChangeScene("GamePlay");
        }

    }

}