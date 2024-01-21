using UnityEngine;
using JusticeRising;
using JusticeRising.GameData;

namespace Tproject
{
    public class MapIconHandler : MonoBehaviour
    {
        public NpcCard npcCard;
        public bool isIntractable;
        public Transform teleportTarget;
        public Camera mapCamera;

        [Space]
        public GameObject targetCam;

        private float initialScale;

        private void Start()
        {
            initialScale = transform.localScale.x;
        }

        private void Update()
        {
            if (mapCamera.gameObject.activeSelf == true)
            {
                float currentZoom = mapCamera.orthographicSize;

                float newScale = Mathf.Floor(currentZoom / 5f) * 1f;

                float finalScale = initialScale + newScale;
                transform.localScale = new Vector3(finalScale, finalScale, finalScale);
            }
            else
            {
                transform.localScale = new Vector3(3f, 3f, 3f);
            }
        }

        void LateUpdate()
        {
            if (LevelManager.instance.CurrentGameState == LevelManager.GameState.Play && targetCam != null)
            {
                Quaternion minimapRotation = targetCam.transform.rotation;

                Vector3 eulerRotation = new Vector3(90f, minimapRotation.eulerAngles.y, 0f);

                transform.rotation = Quaternion.Euler(eulerRotation);
            }
        }

        public NpcCard GetNpcCard()
        {
            return npcCard;
        }

        public Transform GetTeleportDestination()
        {
            return teleportTarget;
        }
    }
}