using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool hasFlyInstructionShownOnceInScene;
    public bool hasMothInstructionShownOnceInScene;
    public bool hasPhantomBugInstructionShownOnceInScene;
    // Start is called before the first frame update
    void Start()
    {
        hasFlyInstructionShownOnceInScene = false;
        hasMothInstructionShownOnceInScene = false;
        hasPhantomBugInstructionShownOnceInScene = false;
        // Ensure ScoreManager is properly reset
        ResetScore();
    }
    void ResetScore()
    {
        // Ensure the ScoreManager is available and reset its score
        ScoreManager scoreManager = ScoreManager.instance;
        if (scoreManager != null)
        {
            scoreManager.ResetScore(); // Reset the score
        }
        else
        {
            Debug.LogError("ScoreManager instance is not available.");
        }
    }
}
