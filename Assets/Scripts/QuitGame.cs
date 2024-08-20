using UnityEngine;
using UnityEngine.SceneManagement; // For Scene management
using UnityEngine.UI; // For Button

public class QuitGame : MonoBehaviour
{
    public Button quitButton; // Reference to the Button

    void Start()
    {
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGameApplication);
        }
        else
        {
            Debug.LogError("Quit button is not assigned.");
        }
    }

    void QuitGameApplication()
    {
#if UNITY_EDITOR
        // If in the Unity editor, stop playing the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If in a standalone build, quit the application
        Application.Quit();
#endif
    }
}
