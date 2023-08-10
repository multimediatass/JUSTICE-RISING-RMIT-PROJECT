using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeRising.VisualNovel
{
    public class DialogManager : MonoBehaviour
    {
        public List<LevelContainer> dialogLevelContainer;

        [System.Serializable]
        public struct LevelContainer
        {
            public int levelID;
            public List<DialogsNPC> DialogsNpc;
        }

        public DialogsNPC GetDialog(string name, int targetLevelID = 0)
        {
            DialogsNPC target;

            LevelContainer desiredLevelContainer = dialogLevelContainer.Find(container => container.levelID == targetLevelID);

            target = desiredLevelContainer.DialogsNpc.Find((npc) => npc.NpcName == name);

            if (target == null)
                Debug.LogWarning($"DialogManager.cs: The NPC {name}'s DialogNPC is EMPTY!!");

            return target;
        }
    }
}
