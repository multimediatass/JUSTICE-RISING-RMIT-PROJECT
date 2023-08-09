using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace JusticeRising
{
    public class Auth : MonoBehaviour
    {
        public TMP_InputField username;
        public TMP_InputField password;

        public void Login()
        {
            if (username.text == "metalabs" && password.text == "123")
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
