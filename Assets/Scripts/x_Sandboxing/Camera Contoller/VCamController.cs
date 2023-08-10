using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeRising
{
    public class VCamController : MonoBehaviour
    {
        [SerializeField] float _Sensitivity;
        [SerializeField] float _MinX;
        [SerializeField] float _MaxX;

        [SerializeField] GameObject _cameraRoot;

        public TouchField _touchField;

        float _xInput;
        float _yInput;

        // Update is called once per frame
        void Update()
        {
            // _xInput += Input.GetAxis("Mouse X");
            // _yInput += Input.GetAxis("Mouse Y") * -1;

            _xInput += _touchField.TouchDist.x * _Sensitivity * 200f * Time.deltaTime;
            _yInput += _touchField.TouchDist.y * -_Sensitivity * 30f * Time.deltaTime;

            _yInput = Mathf.Clamp(_yInput, _MinX, _MaxX);

            _cameraRoot.transform.rotation = Quaternion.Euler(_yInput, _xInput, 0f);
        }
    }
}
