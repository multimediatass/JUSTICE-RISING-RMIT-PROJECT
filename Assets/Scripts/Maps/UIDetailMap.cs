using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JusticeRising;

namespace Tproject
{
    public class UIDetailMap : MonoBehaviour
    {
        public GameObject panel;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        public Image npcImage;

        public Button btnTeleport;

        public PlayerCharacter character;

        private Transform teleportDestination;

        private void Start()
        {
            // Sembunyikan panel saat memulai
            panel.SetActive(false);
        }

        void Update()
        {
            if (panel.activeSelf)
                ValidationTeleport();
        }

        public void ShowDescription(string name, string title, string desc, Sprite img, Transform destination)
        {
            nameText.text = name;
            titleText.text = title;
            descriptionText.text = desc;
            npcImage.sprite = img;

            teleportDestination = destination;

            // Aktifkan panel dengan efek fade in menggunakan LeanTween
            panel.SetActive(true);
            LeanTween.alphaCanvas(panel.GetComponent<CanvasGroup>(), 1f, .5f);
        }

        public void HideDescription()
        {
            // Sembunyikan panel dengan efek fade out menggunakan LeanTween
            LeanTween.alphaCanvas(panel.GetComponent<CanvasGroup>(), 0f, .5f).setOnComplete(() => panel.SetActive(false));
        }

        private void ValidationTeleport()
        {
            if (LevelManager.instance.GetCurrentTimeFloat() >= 30f)
            {
                btnTeleport.interactable = true;
                btnTeleport.GetComponentInChildren<TextMeshProUGUI>().text = $"TALK TO THIS WITNESS";
            }
            else
            {
                btnTeleport.GetComponentInChildren<TextMeshProUGUI>().text = $"WAIT FOR 30 SEC";
                btnTeleport.interactable = false;
            }
        }

        public void OnClickTeleport()
        {
            character.TeleportToDestination(teleportDestination);

            HideDescription();

            LevelManager.instance.PlayGame();
        }
    }
}