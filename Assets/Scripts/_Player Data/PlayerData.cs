using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JusticeRising
{
    [CreateAssetMenu]
    public class PlayerData : ScriptableObject
    {
        public PlayerDataReference dataMap;
    }

    [Serializable]
    public class PlayerDataReference
    {
        public string playerName;
        public string selectedCharacter;
        // public List<CollectedDialog> collectedDialogs;

        // [Serializable]
        // public struct CollectedDialog
        // {
        //     public string npcName;
        //     public List<string> dialogScripts;
        // }
    }
}