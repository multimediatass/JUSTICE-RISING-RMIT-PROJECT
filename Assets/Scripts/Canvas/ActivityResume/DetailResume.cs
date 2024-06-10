using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Tproject.VisualNovelV2;
using JusticeRising.GameData;
using Tproject;

namespace JusticeRising.Canvas
{
    public class DetailResume : MonoBehaviour
    {
        public Image npcImage;
        public TextMeshProUGUI npcName;
        public TextMeshProUGUI npcRole;
        public List<RowItem> rowItems;
        public Transform optParent;
        public NpcCard npcCardDetail;
        public MenuController menuController;

        private void Start()
        {
            StartCoroutine(nameof(ShowConversationList));
        }

        private IEnumerator ShowConversationList()
        {
            yield return new WaitUntil(() => npcCardDetail != null);

            npcImage.sprite = npcCardDetail.npcImages[2];
            npcName.text = npcCardDetail.npcName;
            npcRole.text = npcCardDetail.npcRole;

            int i = 0;
            while (i < npcCardDetail.ConversationSelected.Count + 1)
            {
                if (i == npcCardDetail.ConversationSelected.Count)
                {
                    // bug can't scroll in awake
                    var rowTemp = Instantiate(rowItems[0], optParent);
                    rowTemp.textUI.text = " ";
                    StartCoroutine(DestroyTemp(rowTemp.gameObject));
                }
                else
                {


                    if (i % 2 == 1)
                    {
                        var row = Instantiate(rowItems[1], optParent);
                        row.textUI.text = npcCardDetail.ConversationSelected[i];

                        var Line = Instantiate(rowItems[2], optParent);
                    }
                    else if (i % 2 == 0)
                    {
                        var row = Instantiate(rowItems[0], optParent);
                        row.textUI.text = npcCardDetail.ConversationSelected[i];
                    }
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

        public void OnClickBack()
        {
            StartCoroutine(DestroyTemp(this.gameObject));
            ResumeController.Instance.OnCloseResume();

            menuController.ShowHintTab();
        }
        public void OnClickClose()
        {
            StartCoroutine(DestroyTemp(this.gameObject));
        }
    }
}