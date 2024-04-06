using UnityEngine;
using JusticeRising.GameData;

namespace Tproject
{
    public class MapRaycaster : MonoBehaviour
    {
        public Camera mapCamera;
        public UIDetailMap descriptionController;
        public MapController mapController;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mapCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // Debug.Log(ray);


                if (Physics.Raycast(ray, out hit))
                {
                    // Jika raycast mengenai gedung, tampilkan deskripsi
                    MapIconHandler buildingScript = hit.transform.GetComponent<MapIconHandler>();

                    if (buildingScript != null && buildingScript.isIntractable)
                    {
                        NpcCard targetNpc = buildingScript.GetNpcCard();

                        descriptionController.ShowDescription(targetNpc.npcName, targetNpc.npcRole, targetNpc.npcDescription, targetNpc.npcImages[0], buildingScript.GetTeleportDestination());
                        mapController.HideMainMap();
                    }
                    else if (buildingScript != null && !buildingScript.isIntractable)
                    {
                        descriptionController.OnClickTeleportDirectly(buildingScript.GetTeleportDestination());
                        mapController.HideMainMap();
                    }
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