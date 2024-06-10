using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Tproject.AudioManager;

namespace JusticeRising
{
    [System.Serializable]
    public struct ContentItem
    {
        [TextArea]
        public string storyText;       // Teks untuk cerita
        public Transform[] positions;  // Array posisi kamera
        public AudioClip soundEffect;   // Sound effect untuk bagian cut scene ini
        public float cameraMoveDuration; // Durasi pergerakan kamera
        public UnityEvent onStartStory;   // Event yang dijalankan ketika cerita dimulai
    }

    public class CutSceneManager : MonoBehaviour
    {
        public List<ContentItem> contentItems; // List untuk menyimpan item-item konten cut scene
        public Camera cutSceneCamera; // Referensi ke kamera
        public TextMeshProUGUI storyText;        // Referensi ke UI text untuk menampilkan cerita
        public Image fadeImage;
        public AudioClip backsound;
        // [SerializeField] private AudioSource audioSource;
        [Space]
        public bool isPlayOnStart = false;

        [Space]
        public UnityEvent OnCutsceneStart;
        public UnityEvent OnCutsceneFinished;

        private Coroutine cutSceneCoroutine;

        private bool m_canSkipScene = false;
        public float minWatchDuration = 20f;
        public GameObject canSkipSceneHint;

        void Start()
        {
            // if (audioSource == null)
            //     audioSource = gameObject.AddComponent<AudioSource>();

            if (isPlayOnStart)
                PlayCutScene();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && m_canSkipScene)
            {
                SkipCutScene();
            }
        }

        public void PlayCutScene()
        {
            // audioSource.PlayOneShot(backsound);

            canSkipSceneHint.SetActive(false);
            m_canSkipScene = false;
            Invoke("CanSkipScene", minWatchDuration);

            OnCutsceneStart?.Invoke();

            if (backsound != null) AudioManager.Instance.StartTransitionToNewMusic(backsound, .5f);
            cutSceneCoroutine = StartCoroutine(CutSceneRoutine());
        }

        public void CanSkipScene()
        {
            m_canSkipScene = true;
            canSkipSceneHint.SetActive(true);
        }

        IEnumerator CutSceneRoutine()
        {
            foreach (ContentItem item in contentItems)
            {
                FadeToBlack(true);
                yield return new WaitForSeconds(.5f);
                item.onStartStory?.Invoke();
                FadeToBlack(false);

                if (!string.IsNullOrEmpty(item.storyText))
                {
                    storyText.text = item.storyText;
                }

                if (item.soundEffect != null)
                {
                    // audioSource.PlayOneShot(item.soundEffect);
                    AudioManager.Instance.PlaySFX(item.soundEffect);
                }

                if (item.positions != null && item.positions.Length > 1)
                {
                    for (int i = 0; i < item.positions.Length - 1; i++)
                    {

                        MoveCameraSmoothly(item.positions[i], item.positions[i + 1], item.cameraMoveDuration);
                        yield return new WaitForSeconds(item.cameraMoveDuration);
                    }
                }
            }

            OnCutsceneFinished?.Invoke();
        }

        private void FadeToBlack(bool fadeOut)
        {
            LeanTween.alpha(fadeImage.rectTransform, fadeOut ? 1f : 0f, .5f).setEase(LeanTweenType.easeInOutQuad);
        }

        private void MoveCameraSmoothly(Transform start, Transform target, float duration)
        {
            cutSceneCamera.transform.position = start.position;
            cutSceneCamera.transform.rotation = start.rotation;

            LeanTween.move(cutSceneCamera.gameObject, target.position, duration).setEase(LeanTweenType.easeInOutSine);
            LeanTween.rotate(cutSceneCamera.gameObject, target.rotation.eulerAngles, duration).setEase(LeanTweenType.easeInOutSine);
        }

        private void SkipCutScene()
        {
            if (cutSceneCoroutine != null)
            {
                StopCoroutine(cutSceneCoroutine);
                OnCutsceneFinished?.Invoke();
                cutSceneCoroutine = null;
            }
        }

        public void StopBackSound()
        {
            // audioSource.Stop();
            AudioManager.Instance.StartTransitionToNewMusic("Gameplay Theme", .5f);
        }

        int index = 0;
        public void CheckIndexStory()
        {
            Debug.Log($"show story no: {index}");
            index++;
        }
    }
}