using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Authentication : MonoBehaviour
{
    public RestAPI restAPI;

    [Header("Sign In Components")]
    public TMP_InputField signInEmail;
    public TMP_InputField signInPassword;

    [Header("Sign In Components")]
    public TMP_InputField signUpFirstName;
    public TMP_InputField signUpLastName;
    public TMP_InputField signUpUsername;
    public TMP_InputField signUpEmail;
    public TMP_InputField signUpPassword;
    public TMP_InputField signUpPasswordComfirmation;
    public TMP_InputField signUpPhone;
    public string signUpGender;

    public void OnClickRegister()
    {
        if (string.IsNullOrEmpty(signUpPassword.text) || string.IsNullOrEmpty(signUpPasswordComfirmation.text)
        || string.IsNullOrEmpty(signUpUsername.text) || string.IsNullOrEmpty(signUpEmail.text))
        {
            Debug.LogWarning("please fill the blank");
            return;
        }

        if (signUpPassword.text != signUpPasswordComfirmation.text)
        {
            Debug.LogWarning("comfirm your password");
            return;
        }

        Dictionary<string, string> newPlayer = new Dictionary<string, string>
        {
            {"first_name", signUpFirstName.text},
            {"last_name", signUpLastName.text},
            {"username", signUpUsername.text},
            {"email", signUpEmail.text},
            {"password", signUpPassword.text},
            {"confirm_password", signUpPasswordComfirmation.text},
            {"gender", signUpGender},
            {"phone", signUpPhone.text}
        };

        RowData dataObject = new RowData(newPlayer);
        restAPI.PostAction(dataObject.baseData, "register");
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(signUpPassword.text))
        {
            signUpPasswordComfirmation.interactable = true;
        }
        else
        {
            signUpPasswordComfirmation.interactable = false;
            signUpPasswordComfirmation.text = "";
        }
    }

    public void OnClickGenderSelection(string str)
    {
        signUpGender = str;
        Debug.Log($"player is {str}");
    }

    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(signInEmail.text) || string.IsNullOrEmpty(signInPassword.text))
        {
            Debug.LogWarning("please fill the blank");
            return;
        }

        List<string> c = new List<string>()
        {
            signInEmail.text, signInPassword.text
        };

        Dictionary<string, string> newPlayer = new Dictionary<string, string>
            {
                {"username", signInEmail.text},
                {"password", signInPassword.text}
            };

        RowData dataObject = new RowData(newPlayer);
        restAPI.PostAction(dataObject.baseData, "login");
    }

    private bool OnInputFieldValdation(List<string> valCheck)
    {
        var state = false;

        if (valCheck.Contains(" "))
        {
            state = false;
        }
        else if (valCheck.Contains(""))
        {
            state = false;
        }
        else
        {
            state = true;
        }

        return state;
    }
}
