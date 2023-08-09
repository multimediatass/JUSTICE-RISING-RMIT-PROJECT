using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JusticeRising.VisualNovel;

namespace JusticeRising
{
    public class NPCharacter : Character
    {
        [Header("NPC SETTING")]
        public bool isTalkative = false;
        public DialogHandler _dialogHandler;

        private void Awake()
        {
            if (isTalkative is true)
            {
                _dialogHandler.SetUpDialogHanler(characterName);
            }
        }

    }
}
