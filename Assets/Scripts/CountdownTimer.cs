using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public Damageable damageable;

    [SerializeField]
    private FloatSO timerSO;
    public float RemainingTime => timerSO.Value;

    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerLeftText;

    // Update is called once per frame
    void Update()
    {
        //check if gameOverScreen is not active
        if (!gameOverScreen.activeSelf || !victoryScreen.activeSelf)
        {
            timerSO.Value -= Time.deltaTime;
            DisplayTime(timerSO.Value);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
            // Kills the player
            damageable.IsAlive = false;
        }

        float remainingTime = RemainingTime;
        float timeScore = Mathf.FloorToInt(remainingTime);

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        countdownText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        timerLeftText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
