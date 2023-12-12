using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JusticeRising.GameData;
using UnityEngine.Events;

namespace JusticeRising.Canvas
{
    public class ResumeController : MonoBehaviour
    {
        public static ResumeController Instance;

        public PlayerData playerData;
        public List<NpcCard> resumeList;

        [Header("UI Components")]
        public RowWitnessItem rowPrefab;
        public GameObject resumePanel;
        public Transform contentParent;

        public bool isPlayOnStart;
        [Space]
        public UnityEvent StartOpenResume;
        public UnityEvent AfterCloseResume;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void Start()
        {
            if (isPlayOnStart)
                OnOpenResume();
        }

        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.R) && !resumePanel.activeSelf) OnOpenResume();
        }

        private IEnumerator PrintRowItem()
        {
            resumePanel.SetActive(true);

            for (int child = 0; child < contentParent.childCount; child++)
            {
                Destroy(contentParent.transform.GetChild(child).gameObject);
            }

            yield return new WaitUntil(() => contentParent.childCount == 0);

            int i = 0;
            while (i < resumeList.Count + 1)
            {
                if (i == resumeList.Count)
                {
                    // bug can't scroll in awake
                    var rowTemp = Instantiate(rowPrefab, contentParent);
                    // rowTemp.textUI.text = " ";
                    StartCoroutine(DestroyTemp(rowTemp.gameObject));
                }
                else
                {
                    var item = Instantiate(rowPrefab, contentParent);
                    item.npcImage.sprite = resumeList[i].npcImages[2];
                    item.npcName.text = resumeList[i].npcName;
                    item.npcRole.text = resumeList[i].npcRole;
                    item.npcCardResume = resumeList[i];
                    // item.detailResumeParent = this.transform;
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

        public void OnCloseResume()
        {
            AfterCloseResume?.Invoke();
        }

        public void OnOpenResume()
        {
            // StartOpenResume?.Invoke();

            resumeList.Clear();

            foreach (var item in playerData.npcResumeActivity)
            {
                resumeList.Add(item);
            }

            StartCoroutine(PrintRowItem());
        }
    }
}