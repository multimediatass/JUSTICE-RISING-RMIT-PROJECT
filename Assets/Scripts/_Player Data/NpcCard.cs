using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tproject.VisualNovelV2;

namespace JusticeRising.GameData
{
    [CreateAssetMenu(fileName = "NpcCard", menuName = "GameData/NpcCard")]
    public class NpcCard : ScriptableObject
    {
        public string npcName;
        public string npcRole;
        public List<Sprite> npcImages;
        public DialogsController.DialogScript DialogScripts;
        public List<string> ConversationSelected;

        public void AddConversationSelected(List<string> list)
        {
            foreach (var item in list)
            {
                ConversationSelected.Add(item);
            }
        }
    }
}