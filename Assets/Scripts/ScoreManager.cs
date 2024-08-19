using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // For using the Image component

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton instance to access ScoreManager globally
    public TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component for the current score
    public TextMeshProUGUI highScoreText; // Reference to the TextMeshProUGUI component for the high score
    public SpriteRenderer newHighScoreSprite; // Reference to the SpriteRenderer component to display if a new high score is achieved
    private int score = 0;
    private int highScore;

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
        UpdateScoreText();
        UpdateHighScoreText();

        // Ensure the sprite is initially hidden
        if (newHighScoreSprite != null)
        {
            newHighScoreSprite.gameObject.SetActive(false);
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

            // Display the new high score sprite
            if (newHighScoreSprite != null)
            {
                newHighScoreSprite.gameObject.SetActive(true);
            }
        }
        else
        {
            // Hide the new high score sprite if it's not a new high score
            if (newHighScoreSprite != null)
            {
                newHighScoreSprite.gameObject.SetActive(false);
            }
        }

        UpdateHighScoreText();
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
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        }
        if (highScoreText == null)
        {
            highScoreText = GameObject.Find("HighScoreText")?.GetComponent<TextMeshProUGUI>();
        }

        // Update the score and high score text when a new scene is loaded
        UpdateScoreText();
        UpdateHighScoreText();
    }

    void OnDestroy()
    {
        // Unsubscribe from the scene loaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
