using UnityEngine;
using UnityEngine.UI;
using JusticeRising;
using UnityEngine.Events;

namespace Tproject.CutScene
{
    public class MenuController : MonoBehaviour
    {
        public GameObject container;
        public GameObject[] pages;
        public Button[] menuButtons;
        private int currentPage = 0;
        public UnityEvent OnMenuOpen;
        public UnityEvent AfterMenuClose;


        private void Update()
        {
            if (InputManager.instance.inputAction.PlayerControls.MenuPanel.triggered && !container.activeSelf)
            {
                OnClickOpenMenu();
                OnMenuOpen?.Invoke();
            }
            else if (InputManager.instance.inputAction.PlayerControls.MenuPanel.triggered && container.activeSelf)
            {
                FadeOut(container);
                AfterMenuClose?.Invoke();
                currentPage = 0;
            }
        }

        private void OnClickOpenMenu()
        {
            foreach (var page in pages)
            {
                page.SetActive(false);
            }

            container.SetActive(true);
            FadeIn(container);

            UnHighlightAllButtons();
            OpenMenu(currentPage);
        }

        public void OpenMenu(int pageIndex)
        {
            if (pageIndex >= 0 && pageIndex < pages.Length)
            {
                if (pages[currentPage].activeSelf)
                {
                    FadeOut(pages[currentPage]);
                }

                currentPage = pageIndex;
                FadeIn(pages[currentPage]);

                UnHighlightAllButtons();
                HighlightAllButton(pageIndex);
            }
            else
            {
                Debug.LogError("Page index out of range!");
            }
        }


        public void GoToPage(int pageIndex)
        {
            if (pageIndex >= 0 && pageIndex < pages.Length)
            {
                if (pages[pageIndex].activeSelf)
                {
                    return;
                }
                else
                {
                    FadeOut(pages[currentPage]);
                    currentPage = pageIndex;
                    FadeIn(pages[currentPage]);

                    UnHighlightAllButtons();
                    HighlightAllButton(pageIndex);
                }
            }
            else
            {
                Debug.LogError("Page index out of range!");
            }
        }

        public void NextPage()
        {
            if (currentPage < pages.Length - 1)
            {
                FadeOut(pages[currentPage]);
                currentPage++;
                FadeIn(pages[currentPage]);
            }
        }

        public void PreviousPage()
        {
            if (currentPage > 0)
            {
                FadeOut(pages[currentPage]);
                currentPage--;
                FadeIn(pages[currentPage]);
            }
        }

        private void FadeIn(GameObject page)
        {
            CanvasGroup canvasGroup = page.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = page.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0;
            page.SetActive(true);
            LeanTween.alphaCanvas(canvasGroup, 1, 0.5f);
        }

        private void FadeOut(GameObject page)
        {
            CanvasGroup canvasGroup = page.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = page.AddComponent<CanvasGroup>();

            LeanTween.alphaCanvas(canvasGroup, 0, 0.5f).setOnComplete(() => page.SetActive(false));
        }

        private void HighlightAllButton(int btnIndex)
        {
            if (btnIndex >= 0 && btnIndex < menuButtons.Length)
            {
                menuButtons[btnIndex].image.color = Color.white;
            }
            else
            {
                Debug.LogError($"button with index {btnIndex} out of rang!");
            }
        }

        private void UnHighlightAllButtons()
        {
            foreach (var btn in menuButtons)
            {
                btn.image.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
    }
}