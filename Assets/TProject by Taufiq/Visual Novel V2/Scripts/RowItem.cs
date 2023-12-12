using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Tproject.VisualNovelV2
{
    // Creator Instagram: @shantaufiq

    public class RowItem : MonoBehaviour
    {
        public ItemType rowItemType;

        [Serializable]
        public enum ItemType
        {
            Question, Cright, Cleft, Line
        }

        public TextMeshProUGUI textUI;

        [Header("Row Question")]
        public List<string> questionList;
        [SerializeField] private Transform optParent;
        [SerializeField] private QuestionOption questionOptPrefab;

        private void Start()
        {
            if (rowItemType == ItemType.Question)
                StartCoroutine(nameof(Cloning));
        }

        IEnumerator Cloning()
        {
            int i = 0;
            while (i < questionList.Count + 1)
            {
                if (i == questionList.Count)
                {
                    // bug can't scroll in awake
                    var rowTemp = Instantiate(questionOptPrefab, optParent);
                    rowTemp.option.text = "";
                    StartCoroutine(DestroyTemp(rowTemp.gameObject));
                }
                else
                {
                    var opt = Instantiate(questionOptPrefab, optParent);
                    opt.option.text = questionList[i];
                    opt.btnOptionID = questionList.IndexOf(questionList[i]);
                    opt.action = OnClickQuestionOpt;
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

        public void OnClickQuestionOpt(int optId)
        {
            if (questionList.Count == 0) return;
            // Debug.Log($"player select question number {optId}");

            Destroy(this.gameObject);

            DialogsController.instance.ShowDialogSection(optId);
        }
    }
}