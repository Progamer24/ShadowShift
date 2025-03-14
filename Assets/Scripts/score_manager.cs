using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI References")]
    [SerializeField] private TMP_Text scoreText; // Text to display current score
    [SerializeField] private TMP_Text highScoreText; // Text to display high score

    [Header("Score Settings")]
    [SerializeField] private float scorePerSecond = 2f; // Base score increase per second
    [SerializeField] private int penaltyPoints = 50; // Points lost on failure

    private float score;
    private float highScore;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Load high score
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
        UpdateHighScoreDisplay();
    }

    private void Update()
    {
        // Increase score over time
        score += Time.deltaTime * scorePerSecond;
        scoreText.text = Mathf.FloorToInt(score).ToString();

        // Update high score if necessary
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat("HighScore", highScore);
            UpdateHighScoreDisplay();
        }
    }

    /// <summary>
    /// Applies penalty points on failure.
    /// </summary>
    public void ApplyPenalty()
    {
        score -= penaltyPoints;
        if (score < 0) score = 0;
    }

    /// <summary>
    /// Updates the high score display.
    /// </summary>
    private void UpdateHighScoreDisplay()
    {
        highScoreText.text = $"Best: {Mathf.FloorToInt(highScore)}";
    }
}