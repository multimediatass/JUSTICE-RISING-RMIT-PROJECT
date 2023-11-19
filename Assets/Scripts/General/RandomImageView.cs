using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RandomImageView : MonoBehaviour
{
    public Sprite[] characterSprites; // Array of character sprites
    public Image characterImage; // UI image component to display the character
    private int currentSpriteIndex = 0; // Index of the current sprite

    void OnEnable()
    {
        // Set initial sprite
        characterImage.sprite = characterSprites[currentSpriteIndex];
        // Start the coroutine to change sprites
        StartCoroutine(ChangeCharacterSpriteCoroutine());
    }

    IEnumerator ChangeCharacterSpriteCoroutine()
    {
        // Initially set the image alpha to 1 to ensure it's visible
        characterImage.color = new Color(characterImage.color.r, characterImage.color.g, characterImage.color.b, 1);

        while (true)
        {
            // Wait for 5 seconds
            yield return new WaitForSeconds(3.5f);

            // Fade out the current image, then change the sprite and fade it back in
            LeanTween.alpha(characterImage.rectTransform, 0f, 0.5f).setOnComplete(() =>
            {
                // Change the sprite
                currentSpriteIndex = (currentSpriteIndex + 1) % characterSprites.Length;
                characterImage.sprite = characterSprites[currentSpriteIndex];

                // Immediately set the alpha of the new sprite to 0 (transparent)
                characterImage.color = new Color(characterImage.color.r, characterImage.color.g, characterImage.color.b, 0);

                // Fade in the new image
                LeanTween.alpha(characterImage.rectTransform, 1f, 0.5f);
            });
        }
    }
}
