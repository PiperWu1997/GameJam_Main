using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour
{
    public TextMeshProUGUI highScoreText; // Reference to the TextMeshProUGUI component for displaying the high score
    public TextMeshProUGUI lastScoreText; // Reference to the TextMeshProUGUI component for displaying the last score
    private int highScore; // Persistent high score
    private int lastScore; // Last recorded score

    void Start()
    {
        // Load the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        lastScore = 0; // Initialize the last score to 0

        // Ensure ScoreManager instance is available
        if (ScoreManager.instance != null)
        {
            // Update scores and texts
            UpdateScores();
            UpdateScoreTexts();
        }
        else
        {
            Debug.LogError("ScoreManager instance is not available.");
        }
    }

    void Update()
    {
        // Ensure ScoreManager instance is available
        if (ScoreManager.instance != null)
        {
            // Continuously update scores and texts
            UpdateScores();
            UpdateScoreTexts();
        }
    }

    void UpdateScores()
    {
        int currentScore = ScoreManager.instance.GetCurrentScore();

        // Compare and update scores
        if (currentScore > highScore)
        {
            highScore = currentScore; // Update high score
            PlayerPrefs.SetInt("HighScore", highScore); // Save new high score
            PlayerPrefs.Save();
        }

        lastScore = currentScore; // Update last score
    }

    void UpdateScoreTexts()
    {
        if (highScoreText != null)
        {
            highScoreText.text = highScore.ToString(); // Display high score
        }
        else
        {
            Debug.LogError("HighScoreText is not assigned in the ScoreTracker.");
        }

        if (lastScoreText != null)
        {
            lastScoreText.text = lastScore.ToString(); // Display last score
        }
        else
        {
            Debug.LogError("LastScoreText is not assigned in the ScoreTracker.");
        }
    }
}
