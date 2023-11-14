using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System;

public class GameDataManager : MonoBehaviour
{
    public TextMeshProUGUI message;
    public void OnClickTestingPlayerEvent()
    {
        PlayFabClientAPI.WritePlayerEvent(new WriteClientPlayerEventRequest
        {
            EventName = "visualNovelSelected",
            Body = new Dictionary<string, object>
            {
                {"question1", "Tambah jawaban 1"},
            }
        },
        (s) => Debug.Log($"selected send"),
        (err) => Debug.LogError(err.GenerateErrorReport()));
    }

    public void OnClickGetTime()
    {
        string time = DateTime.Now.ToString("dddd, dd MMMM HH:mm");
        message.text = time;

    }
}
