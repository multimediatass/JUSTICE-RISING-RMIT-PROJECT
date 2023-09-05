using UnityEngine;

namespace JusticeRising
{
    using Tproject.NPC;
    using Tproject.VisualNovelV2;

    public class NPCharacter : Character
    {
        [Header("Intractive Components")]
        public bool isTalkative = false;
        [SerializeField] DialogHandler dialogHandler;
        public GameObject intractiveImage;

        [Header("Trigger Area Settings")]
        public TargetScanner playerScanner;

        [Tooltip("Time in seconde before the Enemy stop pursuing the player when the player is out of sight")]
        public float timeToStopPursuit = 0.5f;

        [SerializeField] private float rangeAttact;
        // [SerializeField] private float NpcSpeed = 0.1f;

        [Header("Player Target")]
        public GameObject _player;

        private void Start()
        {
            dialogHandler.myDialogScript.npcName = characterName;
            IntractiveImageAnimate();
        }

        private void Update()
        {
            FindPlayer();
        }

        public void SetActiveNpc(bool state)
        {
            isTalkative = state;
        }

        private void IntractiveImageAnimate()
        {
            if (intractiveImage.activeSelf == false) return;

            LeanTween.moveY(intractiveImage, 1.9f, 2).setEaseInOutCubic().setLoopPingPong();
        }

        public void FindPlayer()
        {
            //! instantiate player scanner
            GameObject player = playerScanner.DetectPlayer(transform, _player);

            if (player != null)
            {
                float checkPosition = Vector3.Distance(transform.position, player.transform.position);

                if (checkPosition >= rangeAttact)
                {
                    // transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 20 * NpcSpeed * Time.deltaTime);
                    // transform.LookAt(player.transform);
                }

                if (checkPosition < rangeAttact)
                {
                    // Debug.Log("Show popup Intraction");

                    if (isTalkative && LevelManager.instance.CurrentGameState == LevelManager.GameState.Play)
                        dialogHandler.ToggleInstructionPopUp(true);
                }
            }
            else
            {
                // Debug.Log("start roaming");
                dialogHandler.ToggleInstructionPopUp(false);
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            playerScanner.EditorGizmo(transform);
        }
#endif
    }
}
