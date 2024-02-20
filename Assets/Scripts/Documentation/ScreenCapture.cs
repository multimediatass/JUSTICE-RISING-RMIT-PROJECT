using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScreenCapture : MonoBehaviour
{
    [Header("Profile Data")]
    public string FileName;
    public string SavePath;

    public Image finalImage;
    public GameObject CapturePanel;


    IEnumerator SaveFinalImage()
    {
        Debug.Log("you has been captured");

        yield return new WaitForEndOfFrame();

        Sprite sprite = finalImage.sprite;
        Texture2D texture = SpriteToTexture2D(sprite);

        // Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        // screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        // screenImage.Apply();

        // Pastikan SavePath diakhiri dengan "\"
        if (!SavePath.EndsWith(@"\")) SavePath += @"\";

        // Periksa apakah directory tersebut ada, jika tidak, buat
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        // string Path = $"{Application.persistentDataPath}/Customer_{FileName}_{System.DateTime.Now.ToString("yyyyMMdd_HHmmss")}.jpg";
        string Path = $"{SavePath}/Customer_{FileName}_{System.DateTime.Now.ToString("yyyyMMdd_HHmmss")}.jpg";
        Debug.Log(Path);
        byte[] imageBytes = texture.EncodeToJPG();

        File.WriteAllBytes(Path, imageBytes);

        Destroy(texture);

        yield return new WaitForSeconds(0.5f);

        CapturePanel.SetActive(true);
    }

    Texture2D SpriteToTexture2D(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                        (int)sprite.textureRect.y,
                                                        (int)sprite.textureRect.width,
                                                        (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }
}
