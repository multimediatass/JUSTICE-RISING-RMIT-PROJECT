using UnityEngine;
using JusticeRising;
using UnityEngine.UI;

namespace Tproject
{
    public class MapController : MonoBehaviour
    {
        public Camera mapCamera;
        public GameObject player;
        public RawImage mapDisplay;
        public float zoomSpeed = 5f;
        public float minZoom = 5f;
        public float maxZoom = 50f;

        private Vector3 lastPanPosition;
        private bool isPanning;

        public GameObject panelMainMap;

        private void Update()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            ZoomMap(scroll);

            if (IsCursorOverMapDisplay())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    lastPanPosition = Input.mousePosition;
                    isPanning = true;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    isPanning = false;
                }
            }
            else
            {
                isPanning = false;
            }

            if (isPanning)
            {
                PanCamera(Input.mousePosition);
            }
        }

        private bool IsCursorOverMapDisplay()
        {
            return RectTransformUtility.RectangleContainsScreenPoint(
                mapDisplay.rectTransform,
                Input.mousePosition,
                null);
        }

        private void ZoomMap(float increment)
        {
            if (mapCamera.orthographic)
            {
                mapCamera.orthographicSize = Mathf.Clamp(mapCamera.orthographicSize - increment * zoomSpeed, minZoom, maxZoom);
            }
            else
            {
                mapCamera.fieldOfView = Mathf.Clamp(mapCamera.fieldOfView - increment * zoomSpeed, minZoom, maxZoom);
            }
        }

        private void PanCamera(Vector3 newPanPosition)
        {
            // Menghitung perbedaan posisi mouse
            Vector3 offset = mapCamera.ScreenToViewportPoint(lastPanPosition - newPanPosition);

            // Menghitung pergerakan kamera berdasarkan offset
            Vector3 move = new Vector3(-offset.x * mapCamera.orthographicSize, 0, -offset.y * mapCamera.orthographicSize);

            // Memindahkan kamera
            mapCamera.transform.Translate(move, Space.World);
            lastPanPosition = newPanPosition;
        }

        public void ShowMainMap()
        {
            panelMainMap.SetActive(true);

            if (player != null)
            {
                // Mendapatkan posisi x dan z dari pemain (player)
                Vector3 playerPosition = player.transform.position;

                // Mengatur posisi kamera minimap hanya pada sumbu x dan z
                mapCamera.transform.position = new Vector3(playerPosition.x, mapCamera.transform.position.y, playerPosition.z);
            }

            LeanTween.alphaCanvas(panelMainMap.GetComponent<CanvasGroup>(), 1f, .5f);
        }

        public void HideMainMap()
        {
            LeanTween.alphaCanvas(panelMainMap.GetComponent<CanvasGroup>(), 0f, .5f).setOnComplete(() => panelMainMap.SetActive(false));
        }
    }
}