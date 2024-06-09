using Tproject;
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
        public MenuController menuController;

        void Start()
        {
            menuController = GetComponentInParent<MenuController>();
        }

        public void ShowDetail()
        {
            DetailResume detailItem = Instantiate(detailResumePrefab);
            detailItem.npcCardDetail = npcCardResume;
            detailItem.menuController = menuController;
            Debug.Log($"show detail for {npcCardResume.npcName} data");

            ResumeController.Instance.StartOpenResume?.Invoke();

            menuController.hintTab.SetActive(false);
        }

    }
}