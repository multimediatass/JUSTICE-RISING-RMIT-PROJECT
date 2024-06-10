using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class SecondaryCameraController : MonoBehaviour
{
    [System.Serializable]
    public struct CamBehaviour
    {
        public string sceneName;
        public Transform[] camPositions;
        public float duration;
        [Space]
        [Space]
        public UnityEvent OnStartEvent;
        public UnityEvent OnFinishedEvent;
    }

    public List<CamBehaviour> behaviour;
    public GameObject targetCamera;

    public void PlaySecondaryCamera(string _sceneName)
    {
        CamBehaviour arg = behaviour.FirstOrDefault((x) => x.sceneName == _sceneName);

        if (!string.IsNullOrEmpty(arg.sceneName))
        {
            arg.OnStartEvent?.Invoke();

            StartCoroutine(StartCameraMove(arg));
        }
    }

    private IEnumerator StartCameraMove(CamBehaviour param)
    {
        if (param.camPositions != null && param.camPositions.Length > 1)
        {
            float durationPerMove = param.duration / (param.camPositions.Length - 1);

            for (int i = 0; i < param.camPositions.Length - 1; i++)
            {
                // MoveCameraSmoothly(param.camPositions[i], param.camPositions[i + 1], param.duration);
                MoveCameraSmoothly(param.camPositions[i], param.camPositions[i + 1], durationPerMove);
                yield return new WaitForSeconds(durationPerMove);
            }
        }

        param.OnFinishedEvent?.Invoke();
    }

    private void MoveCameraSmoothly(Transform start, Transform target, float duration)
    {
        targetCamera.transform.position = start.position;
        targetCamera.transform.rotation = start.rotation;

        LeanTween.move(targetCamera.gameObject, target.position, duration).setEase(LeanTweenType.easeInOutSine);
        LeanTween.rotate(targetCamera.gameObject, target.rotation.eulerAngles, duration).setEase(LeanTweenType.easeInOutSine);
    }
}
