using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace JusticeRising.VisualNovel
{
    public class RowItem : MonoBehaviour
    {
        public TextMeshProUGUI tmpUI;
        public possition textPos;

        [System.Serializable]
        public enum possition
        {
            left, right
        }
    }
}
