using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text gameOverScoreText;
    public TMP_Text gameOverHighscoreText;
    public TMP_Text victoryScoreText;
    public TMP_Text victoryHighscoreText;

    [SerializeField]
    private FloatSO scoreSO;
    [SerializeField]
    private FloatSO highscoreSO;

    private CountdownTimer countdownTimer;

    // Start is called before the first frame update
    void Start()
    {
        // Find the CountdownTimer in the scene
        countdownTimer = FindObjectOfType<CountdownTimer>();
        // Load highscore from PlayerPrefs.
        LoadHighscore();
        // Update UI to display initial score and highscore
        UpdateScoreUI();
    }

    // Update is called once per frame
    void UpdateScoreUI()
    {
        // Calculate score based on remaining time from countdown timer
        float remainingTime = countdownTimer.RemainingTime;
        float timeScore = Mathf.FloorToInt(remainingTime * 10);

        // Update UI texts
        scoreText.text = "Score: " + scoreSO.Value;
        highScoreText.text = "Highscore: " + highscoreSO.Value;
        gameOverScoreText.text = "Score: " + scoreSO.Value;
        victoryScoreText.text = "Score: " + (scoreSO.Value + timeScore);

        // Check if the current score exceeds the highscore
        if (highscoreSO.Value <= (scoreSO.Value + timeScore))
        {
            scoreText.text = "Score: " + scoreSO.Value + timeScore;
            gameOverHighscoreText.text = "New Highscore: " + highscoreSO.Value;
            victoryHighscoreText.text = "New Highscore: " + (highscoreSO.Value + timeScore);

            // Update highscore value and save it
            highscoreSO.Value += timeScore;
            SaveHighscore();
        }
        else
        {
            gameOverHighscoreText.text = "Highscore: " + highscoreSO.Value;
            victoryHighscoreText.text = "Highscore: " + highscoreSO.Value;
        }
    }

    // Add score points
    public void AddScore(int points)
    {
        scoreSO.Value += points;

        // If the current score is more than the highscore, update highscore
        if (highscoreSO.Value < scoreSO.Value)
        {
            highscoreSO.Value = scoreSO.Value;
        }
        UpdateScoreUI();
    }

    // Save highscore to PlayerPrefs
    private void SaveHighscore()
    {
        PlayerPrefs.SetFloat("Highscore", highscoreSO.Value);
        PlayerPrefs.Save();
    }

    // Load highscore from PlayerPrefs
    private void LoadHighscore()
    {
        if (PlayerPrefs.HasKey("Highscore"))
        {
            highscoreSO.Value = PlayerPrefs.GetFloat("Highscore");
        }
    }
}
