using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JusticeRising.Canvas;

namespace JusticeRising
{
    public class TeleportTrigger : MonoBehaviour
    {
        public Transform teleportDestination;
        public PlayerCharacter character;
        [SerializeField] string actionName;
        GameObject popUpTemp;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && popUpTemp == null)
            {
                popUpTemp = UIManager.instance.ShowInstructionPopUp("[G] Get in office");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (popUpTemp != null)
            {
                Destroy(popUpTemp);
                popUpTemp = null;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (popUpTemp != null)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    character.TeleportToDestination(teleportDestination);
                    Destroy(popUpTemp);
                    popUpTemp = null;
                }
            }
        }
    }
}