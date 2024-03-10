using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    private GameMusic gameMusic;

    // Delay before displaying game over screen
    public float gameOverDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameMusic = GameMusic.instance;
        gameMusic.PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if ((gameOverScreen != null && gameOverScreen.activeInHierarchy) || (victoryScreen != null && victoryScreen.activeInHierarchy))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameMusic.StopMusic();
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void Victory()
    {
        victoryScreen.SetActive(true);
        gameOverScreen.SetActive(true);
    }

    public void GameOver()
    {
        StartCoroutine(ShowGameOverAfterDelay());
    }

    IEnumerator ShowGameOverAfterDelay()
    {
        yield return new WaitForSeconds(gameOverDelay);
        gameOverScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Stage1");

        // Start playing music again when restarting
        gameMusic.PlayMusic();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
