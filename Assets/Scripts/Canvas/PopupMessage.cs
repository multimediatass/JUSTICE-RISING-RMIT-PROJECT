using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PopupMessage : MonoBehaviour
{
    public enum MessageType
    {
        success, error
    }
    public List<TypeContent> messageTypeContents;

    [System.Serializable]
    public struct TypeContent
    {
        public MessageType messageType;
        public Sprite icon;
        public Color bgColor;
    }

    [Space]
    [Header("UI Components")]
    public Image bgPopup;
    public Image iconImage;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI messageText;


    public void ShowMessage(MessageType messageType, string title, string message)
    {
        canvasGroup.alpha = 0;

        var data = messageTypeContents.Find((x) => x.messageType == messageType);
        bgPopup.color = data.bgColor;  // Set background color immediately
        if (bgPopup.color == Color.black)
        {
            Debug.Log("Color is black after assignment. Check alpha and color values.");
        }
        titleText.text = title;
        messageText.text = message;
        iconImage.sprite = data.icon;

        LeanTween.alphaCanvas(canvasGroup, 1f, 0.5f).setEase(LeanTweenType.easeOutQuad);
    }

    public void ResetAlpha() => canvasGroup.alpha = 0;
}
