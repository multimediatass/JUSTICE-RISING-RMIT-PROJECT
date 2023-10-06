using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JusticeRising.GameData;

namespace JusticeRising.Canvas
{
    public class SelectedCard : MonoBehaviour
    {
        public bool isAdded = false;
        public Image UI_witnessImg;
        public TextMeshProUGUI UI_witnessName;

        public NpcCard npcCard;

        public FinalDecisionController finalDecisionController;

        private void Start()
        {
            if (!isAdded)
            {
                DisableComponents();
            }
        }

        public void SetUpSelectedCard(NpcCard _npcCard)
        {
            isAdded = true;
            UI_witnessImg.gameObject.SetActive(true);
            UI_witnessName.gameObject.SetActive(true);

            npcCard = _npcCard;
            UI_witnessImg.sprite = _npcCard.npcImages[1];
            UI_witnessName.text = _npcCard.npcName;
        }

        public void DiselectWitness()
        {
            if (!isAdded) return;

            isAdded = false;
            finalDecisionController.AddUpdateWitnessList(npcCard);

            DisableComponents();
        }

        private void DisableComponents()
        {
            UI_witnessImg.gameObject.SetActive(false);
            UI_witnessName.gameObject.SetActive(false);
        }
    }
}