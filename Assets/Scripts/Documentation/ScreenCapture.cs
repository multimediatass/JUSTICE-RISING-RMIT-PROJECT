using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScreenCapture : MonoBehaviour
{
    private static ScreenCapture instance;

    public Camera myCamera;
    private bool takeScreenshotOnNextFrame;

    private void Awake()
    {
        instance = this;
        if (myCamera == null)
        {
            Debug.LogError("Camera not assigned. Please assign a camera in the inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CaptureScreenshot(1920, 1080);
        }
    }

    IEnumerator CaptureScreenshotCoroutine(int width, int height)
    {
        yield return new WaitForEndOfFrame();

        RenderTexture renderTexture = new RenderTexture(width, height, 16);
        myCamera.targetTexture = renderTexture;
        Texture2D screenImage = new Texture2D(width, height, TextureFormat.ARGB32, false);

        myCamera.Render();

        RenderTexture.active = renderTexture;
        screenImage.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenImage.Apply();

        myCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        byte[] byteArray = screenImage.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Screenshot.png", byteArray);
        Debug.Log($"Screenshot saved to: {Application.dataPath}/Screenshot.png");

        Destroy(screenImage);
    }

    private void TakeScreenshot(int width, int height)
    {
        StartCoroutine(CaptureScreenshotCoroutine(width, height));
    }

    public void CaptureScreenshot(int width, int height)
    {
        if (myCamera == null)
        {
            Debug.LogError("Camera not assigned. Please assign a camera in the inspector.");
            return;
        }
        TakeScreenshot(width, height);
    }
}
