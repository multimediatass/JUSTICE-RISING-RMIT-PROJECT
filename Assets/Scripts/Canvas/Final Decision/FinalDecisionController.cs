using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JusticeRising.GameData;
using System;

namespace JusticeRising.Canvas
{
    public class FinalDecisionController : MonoBehaviour
    {
        public static FinalDecisionController Instance;
        public PlayerData playerData;
        public List<NpcCard> npcList;

        [Header("UI Components")]
        public GameObject Panel;
        public Transform contentParent;
        public RowItemHandler rowPrefab;

        // [Header("UI Components")]
        // public List<


        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void Start()
        {
            foreach (var item in playerData.npcResumeActivity)
            {
                npcList.Add(item);
            }

            StartCoroutine(PrintRowItem());
        }

        private IEnumerator PrintRowItem()
        {
            yield return new WaitUntil(() => npcList.Count == playerData.npcResumeActivity.Count);
            Panel.SetActive(true);

            for (int child = 0; child < contentParent.childCount; child++)
            {
                Destroy(contentParent.transform.GetChild(child).gameObject);
            }

            yield return new WaitUntil(() => contentParent.childCount == 0);

            int i = 0;
            while (i < npcList.Count + 1)
            {
                if (i == npcList.Count)
                {
                    // bug can't scroll in awake
                    var rowTemp = Instantiate(rowPrefab, contentParent);
                    StartCoroutine(DestroyTemp(rowTemp.gameObject));
                }
                else if (i < npcList.Count)
                {
                    var item = Instantiate(rowPrefab, contentParent);

                    int tempIndex = i;
                    // Debug.Log(tempIndex);
                    Action clickAction = () => AddNpcSelection(npcList[tempIndex]);

                    Dictionary<string, object> newItem = new Dictionary<string, object>
                    {
                        {"image", npcList[i].npcImages[2]},
                        {"npcName", npcList[i].npcName},
                        {"npcRole", npcList[i].npcRole},
                        {"npcChard", npcList[i]},
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

        public void AddNpcSelection(NpcCard npc)
        {
            Debug.Log($"Selected npc name: {npc.npcName}, Role {npc.npcRole}");
        }
    }
}