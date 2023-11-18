
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

using JusticeRising;

namespace JusticeRising.Canvas
{
    public class PopUpItem : MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        public TextMeshProUGUI instruction;

        [System.Serializable]
        public struct InspectFormat
        {
            public bool isBuildingInspect;
            public Sprite img;
            public string title;
            [TextArea]
            public string description;
            public string instruction;
        }

        Action afterCloseFunct;

        public void ShowPopUpText(string text)
        {
            if (instruction != null)
                instruction.text = text;
        }

        public void ShowAndDestroyPopUpText(string text)
        {
            if (instruction != null)
                instruction.text = text;

            Destroy(this.gameObject, 3f);
        }

        public void ShowPopUpTextAndIcon(string text, Sprite icon)
        {
            if (image == null && instruction == null) return;

            image.sprite = icon;
            instruction.text = text;
        }

        public void ShowInspectBuilding(PopUpItem.InspectFormat msg)
        {
            if (image == null && instruction == null && description == null && title == null) return;

            image.sprite = msg.img;
            title.text = msg.title;
            description.text = msg.description;
            instruction.text = msg.instruction;
        }

        public void PopUpModal(PopUpItem.InspectFormat msg, Action funct = null)
        {
            if (image == null && instruction == null && description == null && title == null) return;

            afterCloseFunct = funct;

            // image.sprite = msg.img;
            title.text = msg.title;
            description.text = msg.description;
            instruction.text = msg.instruction;
        }

        public void SetUpAction(Action funct)
        {
            afterCloseFunct = funct;
        }

        public void OnClickClose()
        {
            afterCloseFunct?.Invoke();

            Destroy(gameObject);
            Debug.Log($"player has been closed");
        }

        private void Update()
        {
            if (InputManager.instance.inputAction.PlayerControls.Intaction.IsPressed() && afterCloseFunct != null)
            {
                afterCloseFunct.Invoke();
            }
        }
    }
}