using UnityEngine;
using JusticeRising.GameData;

namespace Tproject
{
    public class MapIconHandler : MonoBehaviour
    {

        public NpcCard npcCard;
        public bool isIntractable;
        public Transform teleportTarget;
        public Camera mapCamera;

        private float initialScale;

        private void Start()
        {
            // Simpan skala awal
            initialScale = transform.localScale.x;
        }

        private void Update()
        {
            // Cek jika kamera utama ada
            if (mapCamera.gameObject.activeSelf == true)
            {
                // Dapatkan nilai zoom kamera
                float currentZoom = mapCamera.orthographicSize;

                // Hitung nilai scale berdasarkan kondisi yang diberikan
                float newScale = Mathf.Floor(currentZoom / 5f) * 1f;

                // Set transform scale berdasarkan skala awal dan skala tambahan
                float finalScale = initialScale + newScale;
                transform.localScale = new Vector3(finalScale, finalScale, finalScale);
            }
            else
            {
                transform.localScale = new Vector3(3f, 3f, 3f);
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