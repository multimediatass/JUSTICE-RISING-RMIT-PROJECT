using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeRising.GameData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "GameData/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public PlayerProfileDataMap playerProfile;
        public string playerName;
        public string selectedCharacter;
        public string playerToken;
        public List<NpcCard> npcResumeActivity;

        public List<string> witnessSelected;
    }

    [System.Serializable]
    public class PlayerProfileDataMap
    {
        public string firstName;
        public string lastName;
        public string username;
        public string gender;
        public string email;
        // public string password;
        public string phone;
    }
}