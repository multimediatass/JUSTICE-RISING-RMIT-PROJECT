using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeRising
{
    public class DirectionalArrow : MonoBehaviour
    {
        public static DirectionalArrow instance;
        [SerializeField] private GameObject arrowObj;
        public List<Transform> targetList;
        [SerializeField] private Transform target;
        public float distanceBetweenObjects;
        public bool isLookingForComplate = true;

        private void Awake()
        {
            if (instance == null) instance = this;
        }

        public void RequestLookingForTarget(string targetName)
        {
            var newTarget = targetList.Find((x) => x.name == targetName);
            target = newTarget.transform;

            isLookingForComplate = false;
        }

        private void Update()
        {
            if (!isLookingForComplate)
                LookingForTarget();
            else arrowObj.SetActive(false);
        }

        private void LookingForTarget()
        {
            distanceBetweenObjects = Vector3.Distance(transform.position, target.position);
            Debug.DrawLine(transform.position, target.position, Color.gray);

            var checkingArrow = distanceBetweenObjects > 5f ? true : false;
            arrowObj.SetActive(checkingArrow);

            Vector3 targetPosition = target.transform.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);

            if (distanceBetweenObjects < 2.5f) isLookingForComplate = true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            GUI.color = Color.black;
            UnityEditor.Handles.Label(transform.position - (transform.position - target.position) / 2, distanceBetweenObjects.ToString());
        }
#endif
    }
}