using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace JusticeRising.Canvas
{
    public class PopUpItem : MonoBehaviour
    {
        public TextMeshProUGUI Textfield;

        public void SetUpPopUp(string text)
        {
            Textfield.text = text;
        }
    }
}