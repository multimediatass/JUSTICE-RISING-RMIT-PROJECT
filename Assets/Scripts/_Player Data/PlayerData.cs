using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeRising.GameData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "GameData/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public string playerName;
        public string selectedCharacter;
        public List<NpcCard> npcCards;
    }
}