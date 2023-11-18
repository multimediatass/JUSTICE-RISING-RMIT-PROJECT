using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JusticeRising.Canvas;
using UnityEngine.Events;

namespace JusticeRising
{
    public class TeleportTrigger : MonoBehaviour
    {
        public Transform teleportDestination;
        public PlayerCharacter character;
        GameObject popUpTemp;
        public PopUpItem.InspectFormat inspectFormat;

        public UnityEvent afterTelerport;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && popUpTemp == null)
            {

                if (inspectFormat.isBuildingInspect)
                {
                    popUpTemp = UIManager.instance.ShowBuildingInspector(inspectFormat);
                }
                else
                {
                    popUpTemp = UIManager.instance.ShowInstructionPopUp(inspectFormat.instruction);
                }
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
                if (InputManager.instance.inputAction.PlayerControls.Intaction.IsPressed())
                {
                    character.TeleportToDestination(teleportDestination, () => afterTelerport.Invoke());
                    Destroy(popUpTemp);
                    popUpTemp = null;
                }
            }
        }

        public void CheckGameFinish()
        {

            LevelManager.instance.ChangeGameState(LevelManager.GameState.Pause);
            PopUpItem.InspectFormat msg = new PopUpItem.InspectFormat();
            msg.title = "CAN'T WAIT TO SEE YOU!";
            msg.description = "Please be patient, we're still on our development";
            msg.instruction = "[G] Back to Main Menu";

            popUpTemp = UIManager.instance.ShowPopUpModal(msg, () => LoadingManager.instance.ChangeScene("GameMenu"));
        }
    }
}