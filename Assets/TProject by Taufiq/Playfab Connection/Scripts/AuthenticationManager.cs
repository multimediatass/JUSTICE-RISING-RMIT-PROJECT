using JusticeRising.GameData;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tproject.Authentication
{
    public class AuthenticationManager : MonoBehaviour
    {
        [SerializeField] private bool isForDebug;
        public PlayerData playerData;
        public bool isLoginSection = true;

        [Header("Register Components")]
        public TMP_InputField R_FirstName;
        public TMP_InputField R_LastName;
        // public TextMeshProUGUI R_messageText;
        public TMP_InputField R_Username;
        public TMP_InputField R_emailInput;
        public TMP_InputField R_passwordInput;
        public TMP_InputField R_PasswordComfirmation;
        public TMP_InputField R_Phone;
        [Space]
        public string R_signUpGender;
        public Button[] R_genderButton;
        public Color buttonSelectedColor;
        [Space]
        public PopupMessage R_popupMessage;

        [Space]
        public UnityEvent OnRegisterSuccessEvent;
        public UnityEvent OnRegisterFailedEvent;

        [Header("Login Components")]
        public TMP_InputField L_emailInput;
        public TMP_InputField L_passwordInput;
        [Space]
        public PopupMessage L_popupMessage;
        [Space]
        public UnityEvent OnLoginSuccessEvent;
        public UnityEvent OnLoginFailedEvent;

        [Header("Recovery Account Components")]
        public TextMeshProUGUI Reset_messageText;
        public TMP_InputField Reset_emailInput;
        public GameObject reset_inputPanel;
        public GameObject reset_MessagePanel;

        private void Start()
        {
            if (isForDebug)
            {
                L_emailInput.text = "mytproject2021@gmail.com";
                L_passwordInput.text = "tproject123";
                OnClickLogin();
            }
        }

        private void Update()
        {
            if (!isLoginSection)
            {
                if (!string.IsNullOrEmpty(R_passwordInput.text))
                {
                    R_PasswordComfirmation.interactable = true;
                }
                else
                {
                    R_PasswordComfirmation.interactable = false;
                    R_PasswordComfirmation.text = "";
                }
            }
            else
            {
                // successPopUp.SetActive(false);
                // errPopUp.SetActive(false);
            }
        }

        public bool CheckLoginState()
        {
            return PlayFabClientAPI.IsClientLoggedIn();
        }

        public void OnClickLoginPanelState(bool state)
        {
            isLoginSection = state;
        }

        // Start: Register section
        public void OnClickRegister()
        {
            if (string.IsNullOrEmpty(R_passwordInput.text) || string.IsNullOrEmpty(R_PasswordComfirmation.text)
                || string.IsNullOrEmpty(R_Username.text) || string.IsNullOrEmpty(R_emailInput.text))
            {
                R_popupMessage.ShowMessage(PopupMessage.MessageType.error, "Error", $"Please fill the blank");
                return;
            }

            if (R_passwordInput.text != R_PasswordComfirmation.text)
            {
                // Debug.LogWarning("comfirm your password");
                R_popupMessage.ShowMessage(PopupMessage.MessageType.error, "Error", $"Please comfirm your password");
                return;
            }

            if (R_passwordInput.text.Length < 6)
            {
                R_popupMessage.ShowMessage(PopupMessage.MessageType.error, "Error", $"Input password more than 6 characters");
                return;
            }

            var request = new RegisterPlayFabUserRequest
            {
                DisplayName = R_Username.text,
                Email = R_emailInput.text,
                Password = R_passwordInput.text,
                RequireBothUsernameAndEmail = false
            };

            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterError);
        }

        private void OnRegisterSuccess(RegisterPlayFabUserResult result)
        {
            R_popupMessage.ShowMessage(PopupMessage.MessageType.success, "Success", $"Congratulations! Your account has been successfully created.");

            PlayerProfileDataMap playerData = new PlayerProfileDataMap
            {
                firstName = R_FirstName.text,
                lastName = R_LastName.text,
                username = R_Username.text,
                gender = R_signUpGender,
                email = R_emailInput.text,
                // password = R_passwordInput.text,
                phone = R_Phone.text
            };
            string json = JsonConvert.SerializeObject(playerData);

            UpdateUserData(json);
        }

        void UpdateUserData(string json)
        {
            var data = new Dictionary<string, string>
            {
                {"PlayerProfileData", json}
            };

            var request = new UpdateUserDataRequest
            {
                Data = data
            };

            PlayFabClientAPI.UpdateUserData(request, OnUserDataUpdateSuccess, OnUserDataUpdateError);
        }

        void OnUserDataUpdateSuccess(UpdateUserDataResult result)
        {
            Debug.Log("User data updated successfully!");

            OnRegisterSuccessEvent?.Invoke();
        }

        void OnUserDataUpdateError(PlayFabError error)
        {
            Debug.LogError("Error updating user data: " + error.GenerateErrorReport());
        }

        private void OnRegisterError(PlayFabError error)
        {
            R_popupMessage.ShowMessage(PopupMessage.MessageType.success, "Error", error.ErrorMessage);
            Debug.Log($"{error.GenerateErrorReport()}");
            OnRegisterFailedEvent?.Invoke();
        }

        // End: Register section

        // Start: Login section
        public void OnClickLogin()
        {
            var request = new LoginWithEmailAddressRequest
            {
                Email = L_emailInput.text,
                Password = L_passwordInput.text,

                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
            };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginError);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            string name = null;

            if (result.InfoResultPayload.PlayerProfile.DisplayName != null)
            {
                name = result.InfoResultPayload.PlayerProfile.DisplayName;
            }

            L_popupMessage.ShowMessage(PopupMessage.MessageType.success, "Success", $"player '{name}' has successfully logged");

            GetUserData();
        }

        private void OnLoginError(PlayFabError error)
        {
            L_popupMessage.ShowMessage(PopupMessage.MessageType.error, "Error", $"{error.ErrorMessage}");
            Debug.Log($"{error.GenerateErrorReport()}");

            OnLoginFailedEvent?.Invoke();
        }

        private void GetUserData()
        {
            var request = new GetUserDataRequest();

            PlayFabClientAPI.GetUserData(request, OnGetUserDataSuccess, OnGetUserDataError);
        }

        private void OnGetUserDataError(PlayFabError error)
        {
            Debug.LogError($"Error getting user data: {error.GenerateErrorReport()}");
        }

        private void OnGetUserDataSuccess(GetUserDataResult result)
        {
            if (result.Data != null && result.Data.ContainsKey("PlayerProfileData"))
            {
                string jsonData = result.Data["PlayerProfileData"].Value;
                PlayerProfileDataMap data = JsonConvert.DeserializeObject<PlayerProfileDataMap>(jsonData);
                playerData.playerProfile = data;

                Debug.Log("Data loaded successfully!");

                OnLoginSuccessEvent?.Invoke();
            }
            else
            {
                Debug.LogError("No PlayerProfileData found for this user.");
            }
        }

        // End: Login section

        // Start: Recovery section
        public void OnclickResetPassword()
        {
            var request = new SendAccountRecoveryEmailRequest
            {
                Email = Reset_emailInput.text,
                TitleId = "72269"
            };
            PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnResetError);
        }

        private void OnPasswordReset(SendAccountRecoveryEmailResult result)
        {
            // Debug.Log(result.ToJson());
            Reset_messageText.text = $"We have sent a forgot password email to <color=#C6A03E>{Reset_emailInput.text}</color> account";

            reset_MessagePanel.SetActive(true);
            reset_inputPanel.SetActive(false);
        }

        private void OnResetError(PlayFabError error)
        {
            Debug.Log(error.ErrorMessage);
        }

        public void OpenGmailPage()
        {
            Application.OpenURL("https://mail.google.com/mail/u/0/#inbox");
        }
        // End: Recovery section

        public void OnClickGenderSelection(string gender)
        {
            R_signUpGender = gender;
            Debug.Log($"player select: {gender}");

            // Highlight the selected button
            ResetButtonColors();
            switch (gender)
            {
                case "Male":
                    HighlightButton(R_genderButton[0]);
                    break;
                case "Female":
                    HighlightButton(R_genderButton[1]);
                    break;
                case "Other":
                    HighlightButton(R_genderButton[2]);
                    break;
            }
        }
        void HighlightButton(Button button)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = buttonSelectedColor; // Highlight color
            colors.selectedColor = buttonSelectedColor;
            button.colors = colors;
        }

        public void ResetButtonColors()
        {
            ResetButtonColor(R_genderButton[0]);
            ResetButtonColor(R_genderButton[1]);
            ResetButtonColor(R_genderButton[2]);
        }

        void ResetButtonColor(Button button)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white; // Default color
            button.colors = colors;
        }
    }
}
