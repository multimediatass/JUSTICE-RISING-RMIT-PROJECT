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
        public UnityEvent AfterDialogAction;

        bool isVisVel = false;

        GameObject uiTemp = null;

        private void Start()
        {
            if (isPlayOnStart)
                dialogsController.StartDialog(npcCard, OnAfterDialogAction);
        }

        public void ToggleInstructionPopUp(bool state)
        {
            if (state == true && uiTemp == null && isVisVel == false)
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
                if (Input.GetKeyDown(KeyCode.G)) Dialog();
        }

        private void Dialog()
        {
            dialogsController.StartDialog(npcCard, OnAfterDialogAction);
            // dialogsController.StartDialog(myDialogScript, UpPlayerOpt);
            ToggleInstructionPopUp(false);

            isVisVel = true;

            LevelManager.instance.ChangeGameState(LevelManager.GameState.VisualNovel);
        }

        public void UpPlayerOpt(int opt)
        {
            npcCard.DialogScripts.PlayerOpportunity = opt;
            isVisVel = false;

            AfterDialogAction?.Invoke();
        }

        public void OnAfterDialogAction()
        {
            isVisVel = false;
            AfterDialogAction?.Invoke();
        }
    }
}