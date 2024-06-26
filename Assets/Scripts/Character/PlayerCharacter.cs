using System.Collections;
using Tproject;
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
        [SerializeField] private GameObject[] iconMaps;
        [SerializeField] private GameObject[] introCharacterImg;
        [SerializeField] private CutSceneAnimController animForCutScene;
        GameObject currentCharacter;
        private bool isTeleport = false;
        public Transform defaultPosition;


        private void Awake()
        {
            // SetUpCharacter();
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

        public void SetUpCharacter(string parm)
        {
            int charIndex = -1;
            // charIndex = GetGenderType(GameManager.instance.currentPlayerData.characterSelected);
            charIndex = GetGenderType(parm);

            anim.charAnimIndex = charIndex;
            animForCutScene.SetUpPlayerCutSceneCharacter(charIndex);

            for (int i = charactersPrefabs.Length - 1; i >= 0; i--)
            {
                if (i == charIndex)
                {
                    charactersPrefabs[i].SetActive(true);
                    currentCharacter = charactersPrefabs[i];
                }
                else charactersPrefabs[i].SetActive(false);
            }

            for (int i = iconMaps.Length - 1; i >= 0; i--)
            {
                if (i == charIndex)
                {
                    iconMaps[i].SetActive(true);
                    introCharacterImg[i].SetActive(true);
                }
                else
                {
                    iconMaps[i].SetActive(false);
                    introCharacterImg[i].SetActive(false);
                }
            }

            // LoadingManager.instance.CloseLoadingPanel();
        }

        private void Start()
        {
            _speed = normalSpeed;

            // if (!characterCanMove) anim.SetBoolIsSitting(true);
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

        public void CharacterVisibility(bool state)
        {
            currentCharacter.SetActive(state);
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

        public void TeleportToDestination(Transform destination, Action afterTeleFunct = null)
        {
            LoadingManager.Instance.ShowLoadingScreen(afterTeleFunct, 3f, () => OnTeleport(destination));
            // object[] arg = new object[3] { destination.position, destination.rotation, afterTeleFunct };

            // StartCoroutine(nameof(Teleport), arg);
        }

        public void ResetPlayerPosition()
        {
            OnTeleport(defaultPosition);
        }

        public void OnTeleport(Transform _destination)
        {
            isTeleport = true;
            this.transform.position = _destination.position;
            this.transform.rotation = _destination.rotation;
            Invoke("Teleported", 3f);
        }

        public void Teleported() => isTeleport = false;

        IEnumerator Teleport(object[] parms)
        {
            Action af = (Action)parms[2];
            Quaternion newRotate = (Quaternion)parms[1];

            // LoadingManager.instance.StartLoading();
            isTeleport = true;
            yield return new WaitForSeconds(0.5f);
            this.transform.position = (Vector3)parms[0];
            this.transform.rotation = newRotate;
            yield return new WaitForSeconds(0.5f);
            // Debug.Log($"Player has been teleport {(Vector3)parms[0]}");
            // LoadingManager.instance.CloseLoadingPanel();
            isTeleport = false;
            if (af != null)
                af.Invoke();
        }
    }
}
