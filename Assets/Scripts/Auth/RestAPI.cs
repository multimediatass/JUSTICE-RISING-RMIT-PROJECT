using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class RestAPI : MonoBehaviour
{
    public TargetAPIConfig targetAPIConfig;
    private string endpointTitle = "";

    string tempDataJson;

    #region RestAPI
    public void PostAction(Dictionary<string, string> _data, string _endpointTitle)
    {
        endpointTitle = _endpointTitle;
        var jsonDataToSend = JsonConvert.SerializeObject(_data);

        tempDataJson = jsonDataToSend;
        // Debug.Log(tempDataJson);

        StartCoroutine(nameof(Post));
    }

    IEnumerator Get()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(targetAPIConfig.url + targetAPIConfig.GetEndpoint(endpointTitle)))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
                Debug.LogError(request.error);
            else
            {
                Debug.Log("berhasil");
                string json = request.downloadHandler.text;
                SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(json);
            }
        }
    }

    IEnumerator Post()
    {
        string uri = targetAPIConfig.url + targetAPIConfig.GetEndpoint(endpointTitle);
        Debug.Log($"Start post request to: {uri}");

        using UnityWebRequest webRequest = new UnityWebRequest(uri, "POST");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        byte[] rowData = Encoding.UTF8.GetBytes(tempDataJson);
        webRequest.uploadHandler = new UploadHandlerRaw(rowData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.InProgress:
                break;
            case UnityWebRequest.Result.Success:
                string responseSuccess = webRequest.downloadHandler.text;
                JObject dataSuccess = JObject.Parse(responseSuccess);

                // IList<Msg> returnData = dataSuccess.ToObject<IList<Msg>>();

                Debug.Log(dataSuccess);
                break;
            case UnityWebRequest.Result.ProtocolError:
                string responseProtocolError = webRequest.downloadHandler.text;
                JObject dataResponseProtocolError = JObject.Parse(responseProtocolError);
                Debug.LogError(dataResponseProtocolError);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                string responseDataProcessingError = webRequest.downloadHandler.text;
                JObject dataDataProcessingError = JObject.Parse(responseDataProcessingError);
                Debug.LogError(dataDataProcessingError);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    #endregion
}

[Serializable]
public class RowData
{
    public Dictionary<string, string> baseData;

    public RowData(Dictionary<string, string> _baseData)
    {
        baseData = _baseData;
    }
}

public class Msg
{
    string status { get; set; }
    string message { get; set; }
}