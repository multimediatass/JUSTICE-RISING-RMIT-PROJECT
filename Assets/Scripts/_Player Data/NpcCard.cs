using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeRising.GameData
{
    [CreateAssetMenu(fileName = "NpcCard", menuName = "GameData/NpcCard")]
    public class NpcCard : ScriptableObject
    {
        public string npcName;
        public string npcRole;
        public List<Sprite> npcImages;
        public List<string> ConversationSelected;
    }
}