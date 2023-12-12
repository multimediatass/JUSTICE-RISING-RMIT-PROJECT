using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeRising
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;
        public PlayerActions inputAction;

        private void Awake()
        {
            instance = this;
            inputAction = new PlayerActions();
        }

        private void OnEnable()
        {
            inputAction.PlayerControls.Enable();
        }

        private void OnDisable()
        {
            inputAction.PlayerControls.Enable();
        }
    }
}
