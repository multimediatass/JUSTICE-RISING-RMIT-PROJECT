using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JusticeRising
{
    public class PlayerCharacter : Character
    {
        private float _speed;

        [Header("Camera Controller")]
        [SerializeField] Transform _mainCamera;

        [Header("Character Group")]
        [SerializeField] private GameObject[] charactersPrefabs;

        private bool isTeleport = false;


        private void Awake()
        {
            SetUpCharacter();
        }

        private int GetGenderType(string type)
        {
            string a = type;

            return a switch
            {
                "Male" => 0,
                "Female" => 1,
                "Nonbinary" => 2,
                _ => -1
            };
        }

        private void SetUpCharacter()
        {
            int charIndex = -1;
            charIndex = GetGenderType(GameManager.instance.currentPlayerData.selectedCharacter);

            anim.charAnimIndex = charIndex;

            for (int i = charactersPrefabs.Length - 1; i >= 0; i--)
            {
                if (i == charIndex)
                    charactersPrefabs[i].SetActive(true);
                else charactersPrefabs[i].SetActive(false);
            }

            LoadingManager.instance.CloseLoadingPanel();
        }

        private void Start()
        {
            _speed = normalSpeed;

            if (!characterCanMove) anim.SetBoolIsSitting(true);
        }

        private void Update()
        {
            if (isTeleport) return;

            groundedPlayer = charController.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = -0.5f;
            }

            if (characterCanMove)
            {
                Move();
                Jump();
                // anim.SetBoolIsPlaying(true);
            }
        }

        float animMoveSpeed = 0f;
        float maxAnimMoveSpeed = 0.5f;
        private void Move()
        {
            Vector2 moveVal = InputManager.instance.inputAction.PlayerControls.Movement.ReadValue<Vector2>();

            Vector3 move = _mainCamera.forward * moveVal.y + _mainCamera.right * moveVal.x;
            move.y = 0f;
            charController.Move(_speed * Time.deltaTime * move);

            if (InputManager.instance.inputAction.PlayerControls.SpeedUp.IsPressed())
            {
                _speed = runSpeed;
                maxAnimMoveSpeed = 1f;
            }
            else
            {
                _speed = normalSpeed;
                maxAnimMoveSpeed = 0.5f;

                if (move != Vector3.zero && animMoveSpeed > 0.6f)
                    animMoveSpeed -= 0.05f;
            }

            if (move != Vector3.zero)
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(move), 0.15f);
            }

            if (move != Vector3.zero && animMoveSpeed < maxAnimMoveSpeed)
            {
                animMoveSpeed += 0.09f;
            }
            else if (move == Vector3.zero && animMoveSpeed > 0f)
            {
                animMoveSpeed -= 0.02f;
            }

            anim.MovementValue(animMoveSpeed);
        }

        private void Jump()
        {
            anim.SetBoolGrounded(groundedPlayer);
            anim.SetBoolJumping(false);
            anim.SetBoolFalling(false);

            if (InputManager.instance.inputAction.PlayerControls.Jump.triggered && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

                anim.SetBoolJumping(true);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            charController.Move(playerVelocity * Time.deltaTime);

            if (!groundedPlayer && playerVelocity.y < 0.5f)
            {
                anim.SetBoolFalling(true);
            }
        }

        public void TeleportToDestination(Transform destination, Action afterTeleFunct)
        {
            object[] arg = new object[2] { destination.position, afterTeleFunct };

            StartCoroutine(nameof(Teleport), arg);
        }

        IEnumerator Teleport(object[] parms)
        {
            Action af = (Action)parms[1];

            LoadingManager.instance.StartLoading();
            isTeleport = true;
            yield return new WaitForSeconds(0.5f);
            this.transform.position = (Vector3)parms[0];
            yield return new WaitForSeconds(0.5f);
            // Debug.Log($"Player has been teleport {(Vector3)parms[0]}");
            LoadingManager.instance.CloseLoadingPanel();
            isTeleport = false;
            af.Invoke();
        }
    }
}
