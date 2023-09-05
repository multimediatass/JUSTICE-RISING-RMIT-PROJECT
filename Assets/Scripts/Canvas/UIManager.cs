using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JusticeRising.Canvas
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;
        public PopUpItem popUpAlert;
        public PopUpItem panelTutorial;
        public PopUpItem popUpInspect;
        public PopUpItem popUpFinish;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public GameObject ShowInstructionPopUp(string msg)
        {
            var pop = Instantiate(popUpAlert, this.transform);
            pop.ShowPopUpText(msg);

            return pop.gameObject;
        }

        public void ShowAlertStringMessage(string msg)
        {
            var pop = Instantiate(popUpAlert, this.transform);
            pop.ShowAndDestroyPopUpText(msg);
        }

        public void ShowTutorial(Action actionAfter)
        {
            var pop = Instantiate(panelTutorial, this.transform);
            pop.SetUpAction(actionAfter);
        }

        public GameObject ShowBuildingInspector(PopUpItem.InspectFormat msg)
        {
            var pop = Instantiate(popUpInspect, this.transform);
            pop.ShowInspectBuilding(msg);

            return pop.gameObject;
        }

        public GameObject ShowPopUpModal(PopUpItem.InspectFormat msg, Action nextAction)
        {
            var pop = Instantiate(popUpFinish, this.transform);
            pop.PopUpModal(msg, nextAction);

            return pop.gameObject;
        }
    }
}