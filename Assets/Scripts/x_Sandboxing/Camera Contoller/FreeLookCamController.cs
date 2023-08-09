using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cinemachine;

namespace JusticeRising
{
    public class FreeLookCamController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        Image imgControlArea;
        [SerializeField] CinemachineFreeLook camFreeLook;
        string strMouseX = "Mouse X", strMouseY = "Mouse Y";

        // Start is called before the first frame update
        void Start()
        {
            imgControlArea = GetComponent<Image>();
        }

        private void OnEnable()
        {
            camFreeLook.m_XAxis.m_InputAxisName = null;
            camFreeLook.m_YAxis.m_InputAxisName = null;
            camFreeLook.m_XAxis.m_InputAxisValue = 0;
            camFreeLook.m_YAxis.m_InputAxisValue = 0;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                imgControlArea.rectTransform,
                eventData.position,
                eventData.enterEventCamera,
                out Vector2 postOut
            ))
            {
                // Debug.Log($"the result : {postOut}");
                camFreeLook.m_XAxis.m_InputAxisName = strMouseX;
                camFreeLook.m_YAxis.m_InputAxisName = strMouseY;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            camFreeLook.m_XAxis.m_InputAxisName = null;
            camFreeLook.m_YAxis.m_InputAxisName = null;
            camFreeLook.m_XAxis.m_InputAxisValue = 0;
            camFreeLook.m_YAxis.m_InputAxisValue = 0;
        }
    }
}
