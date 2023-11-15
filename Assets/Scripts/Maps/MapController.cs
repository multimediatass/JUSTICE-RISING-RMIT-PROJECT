using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public Camera mapCamera;
    public RawImage mapDisplay;
    public float zoomSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 50f;

    private Vector3 lastPanPosition;
    private bool isPanning;

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
}
