using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;  // 确保引用了这个命名空间

public class MySceneManager : MonoBehaviour
{
    public GameObject creditsImage;  // Reference to the CreditsImage GameObject
    public SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component
    public float fadeDuration = 1.0f;  // Duration of the fade effect

    public void OnStartButtonClicked()
    {
        StartCoroutine(FadeAndLoadScene("InstructionScene"));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            float elapsedTime = 0f;

            // Set initial transparency to 0
            color.a = 0f;
            spriteRenderer.color = color;

            // Gradually increase transparency to 100%
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                spriteRenderer.color = color;
                yield return null;
            }

            // Ensure transparency is set to 100%
            color.a = 1f;
            spriteRenderer.color = color;
        }

        // Load the MainScene
        SceneManager.LoadScene(sceneName);
    }

    public void OnCreditsButtonClicked()
    {
        if (creditsImage != null)
        {
            creditsImage.SetActive(true);
        }
    }

    public void OnCloseCreditsButtonClicked()
    {
        if (creditsImage != null)
        {
            creditsImage.SetActive(false);
        }
    }

    public void OnStartOverButtonClicked()
    {
        // Load the MainScene and reset the score
        SceneManager.LoadScene("MainScene");
    }
}