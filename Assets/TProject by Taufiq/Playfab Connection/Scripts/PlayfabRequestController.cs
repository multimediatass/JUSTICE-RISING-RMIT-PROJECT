using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class PlayfabRequestController : MonoBehaviour
{
    public DataFormat MyData;

    public List<DataFormat.CoversationData> coversation = new List<DataFormat.CoversationData>
    {
        new DataFormat.CoversationData{questionSetected = "siapa nama anda?", Answer = "NPC Alex"},
        new DataFormat.CoversationData{questionSetected = "berapa sih umurnya?", Answer = "21 tahun"},
        new DataFormat.CoversationData{questionSetected = "apakah script ini benar", Answer = "mudahan sih benar!!!"}
    };

    private void Start()
    {
        DataFormat newData = new DataFormat
        {
            PlayerName = "Muhammad Taufiq",
            Username = "shantaufiq",
            Email = "shantaufiq021@gmail.com",
            SelectedNpcName = "NPC alex",
            NpcRole = "tankk",
            ConversationSelected = coversation,
            SubmitTimeOnGame = "2:15 PM",
            Datestamp = DateTime.Now.ToString("dddd, dd MMMM HH:mm")
        };

        MyData = newData;

        OnLogin();
    }

    private void OnLogin()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = "shantaufiq021@gmail.com",
            Password = "taufiq",

            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithEmailAddress(request,
            (s) => Debug.Log($"success login"),
            (err) => Debug.LogError(err.GenerateErrorReport()));
    }

    public void OnClickConvertToJson()
    {
        string json = JsonConvert.SerializeObject(MyData);

        string str = DateTime.Now.ToString("MMddyy_HHmm");
        Debug.Log(str);

        // UpdateUserData($"DialogWith_{MyData.SelectedNpcName}", json);
    }

    void UpdateUserData(string key, string json)
    {
        var data = new Dictionary<string, string>
            {
                {key, json}
            };

        var request = new UpdateUserDataRequest
        {
            Data = data
        };

        PlayFabClientAPI.UpdateUserData(request,
            (s) => Debug.Log($"success send data to playfab"),
            (err) => Debug.LogError(err.GenerateErrorReport()));
    }
}

[System.Serializable]
public class DataFormat
{
    public string PlayerName;
    public string Username;
    public string Email;
    public string SelectedNpcName;
    public string NpcRole;
    public List<CoversationData> ConversationSelected;
    public string SubmitTimeOnGame;
    public string Datestamp;

    [System.Serializable]
    public struct CoversationData
    {
        public string questionSetected;
        public string Answer;
    }
}