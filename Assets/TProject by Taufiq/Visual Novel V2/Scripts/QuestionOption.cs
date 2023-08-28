using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

namespace Tproject.VisualNovelV2
{
    // Creator Instagram: @shantaufiq

    public class QuestionOption : MonoBehaviour
    {
        public TextMeshProUGUI option;
        public int btnOptionID;
        public Action<int> action;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => action.Invoke(btnOptionID));
            GetComponent<Button>().onClick.AddListener(() => Destroy(this.gameObject));
        }
    }
}