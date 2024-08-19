using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton instance to access ScoreManager globally
    public TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component for the current score
    public TextMeshProUGUI highScoreText; // Reference to the TextMeshProUGUI component for the high score
    public GameObject newHighScoreIndicator; // Reference to the GameObject that indicates a new high score
    private int score = 0; // Current session score
    private int highScore; // Persistent high score

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
        // Load the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        ResetScore(); // Reset score when the game starts
        UpdateScoreText();
        UpdateHighScoreText();

        // Ensure the indicator is initially hidden
        if (newHighScoreIndicator != null)
        {
            newHighScoreIndicator.SetActive(false);
        }

        // Subscribe to scene loading events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    public void EndLevel()
    {
        if (score > highScore)
        {
            // Save the new high score
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();

            // Activate the new high score indicator
            if (newHighScoreIndicator != null)
            {
                newHighScoreIndicator.SetActive(true);
            }
        }
        else
        {
            // Hide the new high score indicator if it's not a new high score
            if (newHighScoreIndicator != null)
            {
                newHighScoreIndicator.SetActive(false);
            }
        }

        UpdateHighScoreText();
    }

    void ResetScore()
    {
        score = 0; // Reset the current session score to 0
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogError("ScoreText is not assigned in the ScoreManager.");
        }
    }

    void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
        }
        else
        {
            Debug.LogError("HighScoreText is not assigned in the ScoreManager.");
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Dynamically find the scoreText and highScoreText in the new scene
        scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        highScoreText = GameObject.Find("HighScoreText")?.GetComponent<TextMeshProUGUI>();

        // Reset score if returning to the game scene
        if (scene.name == "MainScene")
        {
            ResetScore(); // Reset the score for a new game session
        }

        UpdateScoreText();
        UpdateHighScoreText();
    }

    void OnDestroy()
    {
        // Unsubscribe from the scene loaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
