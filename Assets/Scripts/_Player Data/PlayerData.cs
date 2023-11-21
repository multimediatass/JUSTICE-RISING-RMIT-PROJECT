using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeRising.GameData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "GameData/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public PlayerProfileDataMap playerProfile;
        public string characterSelected;

        public List<NpcCard> npcResumeActivity;
        public List<FinalDecisionData.WitnessData> witnessSelected;
        public int PlayerScore;
    }


    [System.Serializable]
    public class FinalDecisionData
    {
        public string PlayerName;
        public string Username;
        public string Email;
        public List<WitnessData> WitnessesSelected;
        public string SubmitTimeOnGame;
        public string Datestamp;

        [System.Serializable]
        public struct WitnessData
        {
            public string WitnessName;
            public string WitnessTitle;

            public WitnessData(string _name, string _title)
            {
                this.WitnessName = _name;
                this.WitnessTitle = _title;
            }
        }
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