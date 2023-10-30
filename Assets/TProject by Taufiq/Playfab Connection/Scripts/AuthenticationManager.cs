
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using TMPro;

namespace Tproject.Authentication
{
    public class AuthenticationManager : MonoBehaviour
    {
        [Header("Register Components")]
        public TMP_InputField R_FirstName;
        public TMP_InputField R_LastName;
        public TextMeshProUGUI R_messageText;
        public TMP_InputField R_Username;
        public TMP_InputField R_emailInput;
        public TMP_InputField R_passwordInput;
        public TMP_InputField R_PasswordComfirmation;
        public TMP_InputField R_Phone;
        public string R_signUpGender;

        [Header("Login Components")]
        public TextMeshProUGUI L_messageText;
        public TMP_InputField L_emailInput;
        public TMP_InputField L_passwordInput;

        [Header("Restart Components")]
        [HideInInspector] public TextMeshProUGUI Reset_messageText;
        [HideInInspector] public TMP_InputField Reset_emailInput;
        [HideInInspector] public TMP_InputField Reset_passwordInput;

        // Start: Register section
        public void OnClickRegister()
        {
            if (R_passwordInput.text.Length < 6)
            {
                R_messageText.text = "Input password more than 6 characters";
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
            R_messageText.text = "Register Success!";

            PlayerProfileDataMap playerData = new PlayerProfileDataMap
            {
                firstName = R_FirstName.text,
                lastName = R_LastName.text,
                username = R_Username.text,
                gender = R_signUpGender,
                email = R_emailInput.text,
                password = R_passwordInput.text,
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
        }

        void OnUserDataUpdateError(PlayFabError error)
        {
            Debug.LogError("Error updating user data: " + error.GenerateErrorReport());
        }

        private void OnRegisterError(PlayFabError error)
        {
            R_messageText.text = error.ErrorMessage;
            Debug.Log($"{error.GenerateErrorReport()}");
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

            L_messageText.text = $"player '{name}' has been successed login!";
        }

        private void OnLoginError(PlayFabError error)
        {
            L_messageText.text = error.ErrorMessage;
            Debug.Log($"{error.GenerateErrorReport()}");
        }

        // End: Login section

        // Start: Recovery section
        public void OnclickResetPassword()
        {
            var request = new SendAccountRecoveryEmailRequest
            {
                Email = Reset_emailInput.text,
                TitleId = "70500"
            };
            PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnResetError);
        }

        private void OnPasswordReset(SendAccountRecoveryEmailResult result)
        {
            Reset_messageText.text = "password reset mail sent";
        }

        private void OnResetError(PlayFabError error)
        {

        }
        // End: Recovery section

        public void OnClickGenderSelection(string str)
        {
            R_signUpGender = str;
            Debug.Log($"player is {str}");
        }
    }

    public class PlayerProfileDataMap
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
    }
}
