using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class IntractionImage : MonoBehaviour
{
    public GameObject _targetCam;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _targetCam.transform.position);
    }
}
