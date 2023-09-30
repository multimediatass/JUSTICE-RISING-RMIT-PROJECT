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
    public TMP_InputField signUpGender;
    public TMP_InputField signUpPhone;

    public void OnClickRegister()
    {
        // if ()
        // {

        // }

        Dictionary<string, string> newPlayer = new Dictionary<string, string>
        {
            {"first_name", signUpFirstName.text},
            {"last_name", signUpLastName.text},
            {"username", signUpUsername.text},
            {"email", signUpEmail.text},
            {"password", signUpPassword.text},
            {"gender", signUpGender.text},
            {"phone", signUpPhone.text}
        };

        RowData dataObject = new RowData(newPlayer);
        restAPI.PostAction(dataObject.baseData, "register");
    }

    public void OnClickLogin()
    {
        List<string> c = new List<string>()
        {
            signInEmail.text, signInPassword.text
        };

        if (OnInputFieldValdation(c))
        {
            Dictionary<string, string> newPlayer = new Dictionary<string, string>
            {
                {"username", signInEmail.text},
                {"password", signInPassword.text}
            };

            RowData dataObject = new RowData(newPlayer);
            restAPI.PostAction(dataObject.baseData, "login");
        }
        else Debug.LogWarning("fill the blank");
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

        // Debug.Log(state);

        return state;
    }
}
