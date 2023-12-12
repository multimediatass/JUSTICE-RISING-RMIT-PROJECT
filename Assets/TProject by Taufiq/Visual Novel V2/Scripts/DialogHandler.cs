using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JusticeRising.GameData;

namespace Tproject.VisualNovelV2
{
    // Creator Instagram: @shantaufiq

    using JusticeRising;

    public class DialogHandler : MonoBehaviour
    {
        public bool isPlayOnStart;
        [SerializeField] DialogsController dialogsController;
        public NpcCard npcCard;
        // public DialogsController.DialogScript myDialogScript;
        [SerializeField] GameObject instructionPrefab;

        [Space]
        [Header("After Dialog Events")]
        [Space]
        public UnityEvent OnValidOpportunity;
        public UnityEvent OnInvalidOpportunity;

        bool isPlaying = false;

        GameObject uiTemp = null;

        private void Start()
        {
            if (isPlayOnStart)
                dialogsController.StartDialog(npcCard, ValidEvent, InvalidEvent);
        }

        public void ToggleInstructionPopUp(bool state)
        {
            if (state == true && uiTemp == null && isPlaying == false)
                uiTemp = Instantiate(instructionPrefab, dialogsController.transform);
            else if (uiTemp != null && state == false)
            {
                // Debug.Log("destroy");
                Destroy(uiTemp);
                uiTemp = null;
            }
        }

        private void Update()
        {
            if (uiTemp != null)
                if (InputManager.instance.inputAction.PlayerControls.Intaction.IsPressed()) Dialog();
        }

        private void Dialog()
        {
            dialogsController.StartDialog(npcCard, ValidEvent, InvalidEvent);
            ToggleInstructionPopUp(false);

            isPlaying = true;

            LevelManager.instance.ChangeGameState(LevelManager.GameState.VisualNovel);
        }

        public void ValidEvent()
        {
            isPlaying = false;
            OnValidOpportunity?.Invoke();
        }

        public void InvalidEvent()
        {
            isPlaying = false;
            OnInvalidOpportunity?.Invoke();
        }

        public void SendCardToGameManager()
        {
            GameManager.instance.AddNpcCard(npcCard);
        }
    }
}