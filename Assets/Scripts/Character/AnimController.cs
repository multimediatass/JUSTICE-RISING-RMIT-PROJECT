using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeRising
{
    public class AnimController : MonoBehaviour
    {
        public int charAnimIndex;
        [SerializeField] private Animator[] animator;

        public void MovementValue(float val)
        {
            animator[charAnimIndex].SetFloat("speed", Mathf.Clamp(val, 0f, 1f));
        }

        public void SetBoolGrounded(bool val)
        {
            animator[charAnimIndex].SetBool("isGrounded", val);
        }

        public void SetBoolJumping(bool val)
        {
            animator[charAnimIndex].SetBool("isJumping", val);
        }

        public void SetBoolFalling(bool val)
        {
            animator[charAnimIndex].SetBool("isFalling", val);
        }

        public void SetBoolIsPlaying(bool val)
        {
            animator[charAnimIndex].SetBool("isPlaying", val);
        }

        public void SetBoolIsSitting(bool val)
        {
            animator[charAnimIndex].SetBool("isSitting", val);
        }
    }
}