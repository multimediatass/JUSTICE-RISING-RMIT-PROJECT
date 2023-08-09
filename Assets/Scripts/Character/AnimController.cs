using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void MovementValue(float val)
    {
        animator.SetFloat("speed", Mathf.Clamp(val, 0f, 1f));
    }

    public void SetBoolGrounded(bool val)
    {
        animator.SetBool("isGrounded", val);
    }

    public void SetBoolJumping(bool val)
    {
        animator.SetBool("isJumping", val);
    }

    public void SetBoolFalling(bool val)
    {
        animator.SetBool("isFalling", val);
    }

    public void SetBoolIsPlaying(bool val)
    {
        animator.SetBool("isPlaying", val);
    }
}
