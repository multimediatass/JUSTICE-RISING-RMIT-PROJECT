using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeRising
{
    public class Character : MonoBehaviour
    {
        public enum GenderType
        {
            Male,
            Female,
            Intersex
        }

        public enum CharacterTpye
        {
            Player, NPC
        }

        public string characterName;
        public GenderType gender;
        public CharacterTpye type;

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
            return $"Name: {characterName}, {gender}, is {type}";
        }

        public void SetCanMove(bool canMove)
        {
            characterCanMove = canMove;
        }
    }
}
