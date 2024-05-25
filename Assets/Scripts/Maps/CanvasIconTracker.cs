using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasIconTracker : MonoBehaviour
{
    public Camera cameraTarget;
    public GameObject canvasTarget;

    [Space]
    public Image prefabImage;
    public Sprite spriteIcon;
    private Image uiUse;
    private Vector3 offset = new Vector3(0, 1.5f, 0);

    private void Start()
    {
        uiUse = Instantiate(prefabImage, canvasTarget.transform).GetComponent<Image>();
        uiUse.sprite = spriteIcon;
    }

    private void Update()
    {
        uiUse.transform.position = cameraTarget.WorldToScreenPoint(this.transform.position + offset);

        float dist = Vector3.Distance(this.transform.position, cameraTarget.transform.position);

        dist = Mathf.Clamp(dist, 0.5f, 1.0f);
        uiUse.transform.localScale = new Vector3(dist, dist, 0);
    }
}
