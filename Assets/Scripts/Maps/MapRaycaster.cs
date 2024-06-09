using UnityEngine;
using UnityEngine.UI;
using JusticeRising.GameData;

namespace Tproject
{
    public class MapRaycaster : MonoBehaviour
    {
        public Camera mapCamera;
        public RawImage mapDisplay;
        public RectTransform mapDisplayRect;
        public UIDetailMap descriptionController;
        public MapController mapController;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 localPoint;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mapDisplayRect, Input.mousePosition, null, out localPoint))
                {
                    // Normalize local coordinates
                    float rectWidth = mapDisplayRect.rect.width;
                    float rectHeight = mapDisplayRect.rect.height;
                    Vector2 normalizedPoint = new Vector2((localPoint.x + rectWidth * 0.5f) / rectWidth, (localPoint.y + rectHeight * 0.5f) / rectHeight);

                    // Convert to viewport point
                    Ray ray = mapCamera.ViewportPointToRay(normalizedPoint);

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        // Hit something in the game world
                        // Debug.Log("Hit " + hit.collider.gameObject.name);

                        MapIconHandler buildingScript = hit.transform.GetComponent<MapIconHandler>();

                        if (buildingScript != null && buildingScript.isIntractable)
                        {
                            NpcCard targetNpc = buildingScript.GetNpcCard();

                            descriptionController.ShowDescription(targetNpc.npcName, targetNpc.npcRole, targetNpc.npcDescription, targetNpc.npcImages[0], buildingScript.GetTeleportDestination());
                            mapController.HideMainMap();
                        }
                        else if (buildingScript != null && !buildingScript.isIntractable && buildingScript.GetTeleportDestination())
                        {
                            descriptionController.OnClickTeleportDirectly(buildingScript.GetTeleportDestination());
                            mapController.HideMainMap();
                        }
                        else
                        {
                            // Jika tidak ada hit, sembunyikan deskripsi
                            descriptionController.HideDescription();

                            Debug.Log("tidak ditemukan");
                        }
                    }
                }
            }
        }
    }
}