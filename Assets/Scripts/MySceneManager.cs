using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public GameObject creditsImage;  // Reference to the CreditsImage GameObject

    public void OnStartButtonClicked()
    {
        // Load the MainScene
        SceneManager.LoadScene("MainScene");
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
