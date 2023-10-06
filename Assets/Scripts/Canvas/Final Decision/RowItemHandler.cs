using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using JusticeRising.GameData;

namespace JusticeRising.Canvas
{
    public class RowItemHandler : MonoBehaviour
    {
        public Dictionary<string, object> rowData = new Dictionary<string, object>();
        public Image Image;
        public List<TextField> textFields;
        public bool isButton, destroyOnClick;
        private Action onClickAction;

        [Serializable]
        public struct TextField
        {
            public string textName;
            public TextMeshProUGUI targetPlace;
        }

        private void Start()
        {
            // Dictionary<string, object> newItem = new Dictionary<string, object>
            // {
            //     {"npcName", "taufiq"},
            //     {"npcRole", "developer"}
            // };

            // rowData = newItem;
            // string gg = rowData.GetData<string>("first_name");
            // Debug.Log($"row data name: {gg}");

            Invoke("SetUpRowItem", 0.1f);
        }

        public void SetUpRowItem()
        {
            Image.sprite = rowData.GetData<Sprite>("image");
            foreach (var item in textFields)
            {
                string fill = rowData.GetData<string>(item.textName);
                item.targetPlace.text = (string)fill;
            }

            onClickAction = rowData.GetData<Action>("onClickAction");
        }

        public void OnClickRowItem()
        {
            if (!isButton) return;

            onClickAction?.Invoke();

            // if (destroyOnClick) Destroy(this.gameObject);
        }
    }

    public static class DataExtensions
    {
        public static T GetData<T>(this Dictionary<string, object> instance, string name)
        {
            return (T)instance[name];
        }

        public static T GetHashtable<T>(this Hashtable table, object key)
        {
            return (T)table[key];
        }
    }
}