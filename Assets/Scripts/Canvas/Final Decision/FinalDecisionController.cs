using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JusticeRising.GameData;
using System;
using UnityEditor;

namespace JusticeRising.Canvas
{
    public class FinalDecisionController : MonoBehaviour
    {
        public static FinalDecisionController Instance;
        public PlayerData playerData;
        public List<NpcCard> witnessList;

        [Header("UI Components")]
        public GameObject Panel;
        public Transform contentParent;
        public RowItemHandler rowPrefab;
        public Button btnSubmitDecision;
        public List<SelectedCard> UI_selectedCard;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void Start()
        {
            foreach (var item in playerData.npcResumeActivity)
            {
                witnessList.Add(item);
            }

            StartCoroutine(PrintRowItem());
        }

        private IEnumerator PrintRowItem()
        {
            // yield return new WaitUntil(() => witnessList.Count == playerData.npcResumeActivity.Count);
            Panel.SetActive(true);

            for (int child = 0; child < contentParent.childCount; child++)
            {
                Destroy(contentParent.transform.GetChild(child).gameObject);
            }

            var submitState = UI_selectedCard.Exists((x) => x.isAdded == false);
            btnSubmitDecision.interactable = !submitState;

            yield return new WaitUntil(() => contentParent.childCount == 0);

            int i = 0;
            while (i < witnessList.Count + 1)
            {
                if (i == witnessList.Count)
                {
                    // bug can't scroll in awake
                    var rowTemp = Instantiate(rowPrefab, contentParent);
                    StartCoroutine(DestroyTemp(rowTemp.gameObject));
                }
                else if (i < witnessList.Count)
                {
                    var item = Instantiate(rowPrefab, contentParent);

                    int tempIndex = i;
                    // Debug.Log(tempIndex);
                    Action clickAction = () => AddWitnessSelection(witnessList[tempIndex]);

                    Dictionary<string, object> newItem = new Dictionary<string, object>
                    {
                        {"image", witnessList[i].npcImages[2]},
                        {"npcName", witnessList[i].npcName},
                        {"npcRole", witnessList[i].npcRole},
                        {"npcChard", witnessList[i]},
                        {"onClickAction", clickAction}
                    };

                    item.rowData = newItem;
                }

                yield return new WaitForSeconds(0.01f);
                i++;
            }
        }

        IEnumerator DestroyTemp(GameObject go)
        {
            yield return new WaitUntil(() => go != null);
            Destroy(go);
            // Debug.Log($"destroy {go}");
        }

        public void AddWitnessSelection(NpcCard npc)
        {
            var newItem = UI_selectedCard.Find((x) => x.isAdded == false);

            if (newItem)
            {
                witnessList.Remove(npc);

                newItem.SetUpSelectedCard(npc);
                // Debug.Log($"Selected npc name: {npc.npcName}, Role {npc.npcRole}");
                StartCoroutine(PrintRowItem());
            }
            else
            {
                Debug.LogWarning($"you only has 3");
            }
        }

        public void AddUpdateWitnessList(NpcCard npc)
        {
            witnessList.Add(npc);
            StartCoroutine(PrintRowItem());
        }
    }
}