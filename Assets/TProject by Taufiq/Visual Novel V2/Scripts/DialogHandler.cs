using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tproject.VisualNovelV2
{
    // Creator Instagram: @shantaufiq

    using JusticeRising;

    public class DialogHandler : MonoBehaviour
    {
        [SerializeField] DialogsController dialogsController;
        public DialogsController.DialogScript myDialogScript;
        [SerializeField] GameObject instructionPrefab;
        public UnityEvent AfterDialogAction;

        bool isVisVel = false;

        GameObject uiTemp = null;

        public void ToggleInstructionPopUp(bool state)
        {
            if (state == true && uiTemp == null && isVisVel == false)
                uiTemp = Instantiate(instructionPrefab, dialogsController.transform);
            else if (uiTemp != null && state == false)
            {
                Debug.Log("destroy");
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
            dialogsController.StartDialog(myDialogScript, UpPlayerOpt);
            ToggleInstructionPopUp(false);

            isVisVel = true;

            LevelManager.instance.ChangeGameState(LevelManager.GameState.VisualNovel);
        }

        public void UpPlayerOpt(int opt)
        {
            myDialogScript.PlayerOpportunity = opt;
            isVisVel = false;

            AfterDialogAction?.Invoke();
        }
    }
}