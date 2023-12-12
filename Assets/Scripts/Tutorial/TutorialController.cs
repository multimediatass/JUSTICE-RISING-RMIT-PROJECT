using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Tproject;

[System.Serializable]
public class ImageData
{
    public string imageTitle;
    public string Section;
    public Sprite sprite;
}

public class TutorialController : MonoBehaviour
{
    public MenuController menuController;
    public Image screenBlocker;
    public GameObject panelContainer;
    public float animationTime = 0.5f;
    public List<ImageData> imageDataList;
    private int currentIndex = 0;
    public Image displayImage;
    public TextMeshProUGUI titleText;

    public Button nextButton;
    public Button previousButton;

    [SerializeField] private List<ImageData> filteredList;
    private string currentSection;
    private bool isOpenInMenu = false;
    private int tutorDefaultIndex = 0;

    [Space]
    public UnityEvent OnTutorStart;
    public UnityEvent OnTutorClosed;

    private string GetTutorSection(int _index)
    {
        return _index switch
        {
            1 => "controller",
            2 => "interaction",
            3 => "map",
            4 => "finalDecision",
            _ => "finished"
        };
    }

    void Start()
    {
        if (panelContainer != null)
            panelContainer.SetActive(false);

        // Initialize with some default section if desired
        // FilterBySection("finalDecision");
    }

    public void RestartTutorDefaultIndex()
    {
        tutorDefaultIndex = 0;
    }

    public void OpenTutorDefault(int stepIndex)
    {
        if (stepIndex <= tutorDefaultIndex)
        {
            OnTutorClosed?.Invoke();
        }
        else
        {
            OpenTutorBySection(GetTutorSection(stepIndex));
            tutorDefaultIndex++;
        }
    }

    public void OnClickOpenTutor(string tutorSection)
    {
        OpenTutorBySection(tutorSection);
        isOpenInMenu = true;

        menuController.DetailOpenState(true);
    }

    private void OpenTutorBySection(string filterName)
    {
        if (panelContainer != null)
        {
            screenBlocker.enabled = true;
            panelContainer.SetActive(true);

            OnTutorStart?.Invoke();

            // Mengatur posisi awal panelContainer di luar layar
            RectTransform rectTransform = panelContainer.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -Screen.height);

            // Mengatur posisi akhir panelContainer di tengah layar
            Vector2 endPosition = new Vector2(rectTransform.anchoredPosition.x, 0);

            // Animasi LeanTween untuk RectTransform
            LeanTween.value(panelContainer, rectTransform.anchoredPosition, endPosition, animationTime)
                .setEase(LeanTweenType.easeOutQuad)
                .setOnUpdate((Vector2 val) => { rectTransform.anchoredPosition = val; });

            FilterBySection(filterName); // Panggil fungsi filter
        }
    }

    public void CloseTutor()
    {
        if (panelContainer != null)
        {
            // Mengatur posisi akhir panelContainer di luar layar
            RectTransform rectTransform = panelContainer.GetComponent<RectTransform>();
            Vector2 endPosition = new Vector2(rectTransform.anchoredPosition.x, -1011);

            // Animasi LeanTween untuk RectTransform
            LeanTween.value(panelContainer, rectTransform.anchoredPosition, endPosition, animationTime)
                .setEase(LeanTweenType.easeInQuad)
                .setOnUpdate((Vector2 val) => { rectTransform.anchoredPosition = val; })
                .setOnComplete(() => { panelContainer.SetActive(false); }); // Nonaktifkan panelContainer setelah animasi selesai

            screenBlocker.enabled = false;

            if (!isOpenInMenu)
                OnTutorClosed?.Invoke();
            else
            {
                isOpenInMenu = false;
                menuController.DetailOpenState(false);
            }
        }
    }

    private void FilterBySection(string section)
    {
        currentSection = section;
        filteredList = imageDataList.Where(imgData => imgData.Section == section).ToList();
        currentIndex = 0;

        if (filteredList.Count > 0)
        {
            UpdateImageDisplay();
        }
        else
        {
            // Handle case where no images match the filter
            displayImage.sprite = null;
            titleText.text = "No images available";
            // sectionText.text = "";
        }
    }

    public void NextImage()
    {
        if (filteredList.Count > 0)
        {
            currentIndex = (currentIndex + 1) % filteredList.Count;
            UpdateImageDisplay();
        }
    }

    public void PreviousImage()
    {
        if (filteredList.Count > 0)
        {
            currentIndex = (currentIndex - 1 + filteredList.Count) % filteredList.Count;
            UpdateImageDisplay();
        }
    }

    private void UpdateButtonStates()
    {
        // Update the previous button state and color
        if (currentIndex == 0)
        {
            previousButton.interactable = false;
            // SetButtonAlpha(previousButton, 100);
        }
        else
        {
            previousButton.interactable = true;
            // SetButtonAlpha(previousButton, 255);
        }

        // Update the next button state and color
        if (currentIndex >= filteredList.Count - 1)
        {
            nextButton.interactable = false;
            // SetButtonAlpha(nextButton, 100);
        }
        else
        {
            nextButton.interactable = true;
            // SetButtonAlpha(nextButton, 255);
        }
    }

    private void SetButtonAlpha(Button button, byte alpha)
    {
        Color color = button.image.color;
        color.a = alpha / 255f;
        button.image.color = color;
    }


    private void UpdateImageDisplay()
    {
        ImageData currentImage = filteredList[currentIndex];
        displayImage.sprite = currentImage.sprite;
        titleText.text = currentImage.imageTitle;
        // sectionText.text = currentImage.Section;

        UpdateButtonStates();
    }
}
