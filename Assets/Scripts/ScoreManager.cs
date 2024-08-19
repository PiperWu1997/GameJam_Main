using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton instance to access ScoreManager globally
    public TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component for displaying the score
    private int score = 0; // Current session score

    void Awake()
    {
        // Singleton pattern to ensure only one instance of ScoreManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Preserve this object across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize the score and update the displayed number
        ResetScore();
        UpdateScoreText();
    }

    public int GetCurrentScore()
    {
        return score; // Return the current session score
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    public void ResetScore()
    {
        score = 0; // Reset the current session score to 0
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText == null)
        {
            // Attempt to find the scoreText object in the scene
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        }

        if (scoreText != null)
        {
            scoreText.text = score.ToString(); // Display only the score number
        }
        else
        {
            Debug.LogError("ScoreText is not assigned in the ScoreManager.");
        }
    }
}
