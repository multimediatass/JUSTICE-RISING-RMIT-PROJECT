using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JusticeRising
{
    public class PlayerCharacter : Character
    {
        // input fields
        private PlayerActions _playerActionAsset;
        private float _speed;

        [Header("Camera Controller")]
        [SerializeField] Transform _mainCamera;


        private void Awake()
        {
            _playerActionAsset = new PlayerActions();
        }

        private void OnEnable()
        {
            _playerActionAsset.PlayerControls.Enable();
        }

        private void OnDisable()
        {
            _playerActionAsset.PlayerControls.Disable();
        }

        private void Start()
        {
            _speed = normalSpeed;
        }

        private void Update()
        {
            groundedPlayer = charController.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = -0.5f;
            }

            if (characterCanMove)
            {
                Move();
                Jump();
                anim.SetBoolIsPlaying(true);
            }
        }

        float animMoveSpeed = 0f;
        float maxAnimMoveSpeed = 0.5f;
        private void Move()
        {
            Vector2 moveVal = _playerActionAsset.PlayerControls.Movement.ReadValue<Vector2>();

            Vector3 move = (_mainCamera.forward * moveVal.y + _mainCamera.right * moveVal.x);
            move.y = 0f;
            charController.Move(_speed * Time.deltaTime * move);

            if (_playerActionAsset.PlayerControls.SpeedUp.IsPressed())
            {
                _speed = runSpeed;
                maxAnimMoveSpeed = 1f;
            }
            else
            {
                _speed = normalSpeed;
                maxAnimMoveSpeed = 0.5f;

                if (move != Vector3.zero && animMoveSpeed > 0.6f)
                    animMoveSpeed -= 0.02f;
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

            if (_playerActionAsset.PlayerControls.Jump.triggered && groundedPlayer)
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
    }
}
