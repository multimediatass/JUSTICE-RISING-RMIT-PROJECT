using UnityEngine;
using JusticeRising.Canvas;
using UnityEngine.Events;

namespace JusticeRising
{
    public class EnterBuildingController : MonoBehaviour
    {
        GameObject popUpTemp;
        public PopUpItem.InspectFormat inspectFormat;

        public UnityEvent OnPlayerEnter;

        void OnTriggerEnter(Collider other)
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

        void OnTriggerExit(Collider other)
        {
            ClosePopup();
        }

        private void ClosePopup()
        {
            if (popUpTemp != null)
            {
                Destroy(popUpTemp);
                popUpTemp = null;
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (popUpTemp != null)
            {
                if (InputManager.instance.inputAction.PlayerControls.Intaction.IsPressed())
                {
                    OnPlayerEnter?.Invoke();
                    Destroy(popUpTemp);
                    popUpTemp = null;
                }
            }

            if (InputManager.instance.inputAction.PlayerControls.MenuPanel.triggered || InputManager.instance.inputAction.PlayerControls.Maps.triggered)
            {
                ClosePopup();
            }
        }
    }
}