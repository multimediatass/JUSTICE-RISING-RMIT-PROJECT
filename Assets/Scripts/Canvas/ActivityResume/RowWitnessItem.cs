using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JusticeRising.GameData;

namespace JusticeRising.Canvas
{
    public class RowWitnessItem : MonoBehaviour
    {
        public Image npcImage;
        public TextMeshProUGUI npcName;
        public TextMeshProUGUI npcRole;
        public NpcCard npcCardResume;
        [HideInInspector] public Transform detailResumeParent;
        public DetailResume detailResumePrefab;

        public void ShowDetail()
        {
            DetailResume detailItem = Instantiate(detailResumePrefab, detailResumeParent);
            detailItem.npcCardDetail = npcCardResume;
            Debug.Log($"show detail for {npcCardResume.npcName} data");
        }

    }
}