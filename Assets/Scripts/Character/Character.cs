using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JusticeRising
{
    public class Character : MonoBehaviour
    {
        [Serializable]
        public enum GenderType
        {
            Male,
            Female,
            Intersex
        }

        // public GenderType gender;
        public string characterName;

        [Header("Character Controller")]
        public AnimController anim;
        public CharacterController charController;
        public bool characterCanMove = false;

        public bool groundedPlayer;
        public float gravityValue = -9.81f;
        public Vector3 playerVelocity;

        [Range(0, 10)]
        public float normalSpeed = 2.0f;

        [Range(0, 10)]
        public float runSpeed = 4.0f;
        public float jumpHeight = 1.0f;

        public string GetCharacterInfo()
        {
            return $"Name: {characterName}";
        }

        public void SetCanMove(bool canMove)
        {
            characterCanMove = canMove;

            if (canMove) anim.SetBoolIsPlaying(true);
        }
    }
}
