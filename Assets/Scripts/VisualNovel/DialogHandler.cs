using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeRising.VisualNovel
{
    public class DialogHandler : MonoBehaviour
    {
        private string NpcName;

        [Header("Visual Novel")]
        [SerializeField] private DialogManager _dialogManager;
        [SerializeField] DialogCanvasController _dialogController;
        private DialogsNPC myDialogsNPC;
        public GameObject btnIntractionPrefab;
        [SerializeField] private GameObject _btnIntraction;
        public bool isMouseIntraction = true;

        public void SetUpDialogHanler(string name)
        {
            NpcName = name;
            if (_dialogManager.GetDialog(name, 1) != null)
                myDialogsNPC = _dialogManager.GetDialog(name, 1);
        }

        private void GiveMeQuestion()
        {
            if (myDialogsNPC != null)
                _dialogController.OnClickOpenQuestion(myDialogsNPC);
            else
            {
                _dialogController.OnClickOpenQuestion();
                Debug.LogWarning($"NFCharacter.cs: NPC {NpcName} doesn't have dialog");
            }

            Destroy(_btnIntraction);
            _btnIntraction = null;

            //! send to level manager the dialog has been started
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _btnIntraction = Instantiate(btnIntractionPrefab, _dialogController.transform, false);

                if (isMouseIntraction)
                    _btnIntraction.GetComponent<Button>().onClick.AddListener(delegate { GiveMeQuestion(); });
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (!isMouseIntraction)
                    if (_btnIntraction != null && InputManager.instance._playerActionAsset.PlayerControls.Intaction.IsPressed()) GiveMeQuestion();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Destroy(_btnIntraction, 0.5f);
                _btnIntraction = null;
            }
        }
    }
}
