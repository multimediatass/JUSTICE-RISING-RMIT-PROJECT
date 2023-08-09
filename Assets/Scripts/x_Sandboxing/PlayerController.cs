using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Controller Settings")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraMain;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;

    [Range(0, 10)]
    public float playerSpeed = 5.0f; // default
    [SerializeField] float currentSpeed;
    public float jumpHeight = 1.0f;

    // move val from player input
    Vector2 moveVal = Vector2.zero;

    private void Start()
    {
        currentSpeed = playerSpeed;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Move();
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        moveVal = value.ReadValue<Vector2>();
    }

    public void OnIntraction(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            Debug.Log($"Player is attacing: {value.started}");
        }
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            Jump();
        }
    }

    public void OnSpeedUp(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            SpeedUp();
        }
        else if (value.canceled)
        {
            currentSpeed = playerSpeed;
        }
    }

    private void Move()
    {
        Vector3 move = (cameraMain.forward * moveVal.y + cameraMain.right * moveVal.x);
        move.y = 0f;
        controller.Move(move * Time.deltaTime * currentSpeed);

        if (move != Vector3.zero)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(move), 0.15f);
        }

        // Debug.Log($"Current speed: {currentSpeed}");
    }

    public void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }

    public void SpeedUp()
    {
        currentSpeed = 10f;
    }
}

