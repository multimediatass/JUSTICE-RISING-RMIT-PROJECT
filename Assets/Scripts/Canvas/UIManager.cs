using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeRising.Canvas
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;
        public PopUpItem popUpItem;

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
            var pop = Instantiate(popUpItem, this.transform);
            pop.SetUpPopUp(msg);

            return pop.gameObject;
        }
    }
}