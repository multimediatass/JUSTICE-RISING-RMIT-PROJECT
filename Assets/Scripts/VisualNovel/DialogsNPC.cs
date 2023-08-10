using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeRising.VisualNovel
{
    [CreateAssetMenu(fileName = "NewNPCDialog", menuName = "NPC/NewNPCDialog")]
    [System.Serializable]
    public class DialogsNPC : ScriptableObject
    {
        public string NpcName;
        public List<SentenceItem> Sentences;

        [System.Serializable]
        public struct SentenceItem
        {
            public string nameSpeaker;
            public Sprite imageSpeaker;

            [TextArea]
            public string dialogScript;
            public AudioClip SFX;
        }

        public List<string> GetListDialogScript()
        {
            List<string> result = new List<string>();

            foreach (JusticeRising.VisualNovel.DialogsNPC.SentenceItem item in Sentences)
            {
                result.Add(item.dialogScript);
            };

            return result;
        }
    }
}
