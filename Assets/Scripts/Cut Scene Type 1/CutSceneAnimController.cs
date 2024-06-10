using System.Collections;
using System.Collections.Generic;
using JusticeRising;
using UnityEngine;
using UnityEngine.Events;

public class CutSceneAnimController : MonoBehaviour
{
    [System.Serializable]
    public struct AnimationBehaviour
    {
        public string animationName;
        public string triggerName;
        public Transform[] positions;
        public float animationDuration;
        public UnityEvent onAnimationComplete;
    }

    public List<AnimationBehaviour> animationBehaviours;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject objectToMove;

    [SerializeField] bool isPlayer;
    [SerializeField] private Animator[] playerAnim;

    // private AnimationBehaviour onBehaviourPlay = new AnimationBehaviour();
    private Coroutine myCoroutine;

    public void SetUpPlayerCutSceneCharacter(int index)
    {
        if (isPlayer)
            animator = playerAnim[index];

        for (int i = playerAnim.Length - 1; i >= 0; i--)
        {
            if (i == index)
                playerAnim[i].gameObject.SetActive(true);
            else playerAnim[i].gameObject.SetActive(false);
        }
    }

    public void SetToIdle() => animator.SetTrigger("idle");

    public void TriggerAnimation(string contentName)
    {
        foreach (var behaviour in animationBehaviours)
        {
            if (behaviour.animationName == contentName)
            {
                animator.SetTrigger(behaviour.triggerName);
                myCoroutine = StartCoroutine(WaitForAnimation(behaviour));
                LevelManager.instance.PlayCutScene();
                break;
            }
        }
    }

    private IEnumerator WaitForAnimation(AnimationBehaviour behaviour)
    {
        if (behaviour.positions != null && behaviour.positions.Length >= 2)
        {
            float durationPerMove = behaviour.animationDuration / (behaviour.positions.Length - 1);

            for (int i = 0; i < behaviour.positions.Length - 1; i++)
            {
                MoveObjectSmoothly(behaviour.positions[i], behaviour.positions[i + 1], durationPerMove);

                yield return new WaitForSeconds(durationPerMove);
            }
        }
        else if (behaviour.positions.Length == 1)
        {
            objectToMove.transform.position = behaviour.positions[0].position;
            objectToMove.transform.rotation = behaviour.positions[0].rotation;

            yield return new WaitForSeconds(behaviour.animationDuration);
        }

        // yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // animator.SetTrigger("idle");
        behaviour.onAnimationComplete.Invoke();
    }

    private void MoveObjectSmoothly(Transform start, Transform target, float totalDuration)
    {
        objectToMove.transform.position = start.position;
        objectToMove.transform.rotation = start.rotation;

        float moveDuration = totalDuration * 0.8f; // 80% dari total durasi untuk pergerakan
        float rotateDuration = totalDuration * 0.2f; // 20% dari total durasi untuk rotasi

        // Mulai pergerakan
        LeanTween.move(objectToMove, target.position, totalDuration).setEase(LeanTweenType.easeInOutSine);

        // Mulai rotasi setelah pergerakan selesai
        LeanTween.value(gameObject, 0f, 1f, moveDuration).setOnComplete(() =>
        {
            LeanTween.rotate(objectToMove, target.rotation.eulerAngles, rotateDuration).setEase(LeanTweenType.easeInOutSine);
        });
    }

    public void FinishThisAnimation()
    {
        if (myCoroutine != null)
        {
            SetToIdle();
            StopCoroutine(myCoroutine);
            myCoroutine = null;
            this.gameObject.SetActive(false);
        }
    }
}
