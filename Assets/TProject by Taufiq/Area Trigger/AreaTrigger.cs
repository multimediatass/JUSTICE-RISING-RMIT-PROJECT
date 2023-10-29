using UnityEngine.Events;
using UnityEngine;
using JusticeRising;

namespace Tproject
{
    public class AreaTrigger : MonoBehaviour
    {
        public string targetCharacterName;

        public UnityEvent OnTargetEnter;
        [Space]
        public UnityEvent OnTargetExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Character>(out Character car))
            {
                if (car.characterName == targetCharacterName)
                    OnTargetEnter?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Character>(out Character car))
            {
                if (car.characterName == targetCharacterName)
                    OnTargetExit?.Invoke();
            }
        }
    }
}