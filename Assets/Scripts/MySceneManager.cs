using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public GameObject creditsImage;  // 拖拽 CreditsImage 对象到此字段

    public void OnStartButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
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
}
