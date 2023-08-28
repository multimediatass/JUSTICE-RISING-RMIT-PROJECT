using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeRising
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;
        public PlayerActions _playerActionAsset;

        private void Awake()
        {
            instance = this;
            _playerActionAsset = new PlayerActions();
        }

        private void OnEnable()
        {
            _playerActionAsset.PlayerControls.Enable();
        }

        private void OnDisable()
        {
            _playerActionAsset.PlayerControls.Enable();
        }
    }
}
