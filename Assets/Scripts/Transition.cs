using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public GameObject circleExpand;
    public GameManager gameManager;
    public Boss boss;
    public Animator transition;
    public float transitionTime = 5f;

    // Update is called once per frame
    void Update()
    {
        if (boss.IsDestroyed())
        {
            circleExpand.SetActive(true);
            LoadVictoryScreen();
        }
    }

    public void LoadVictoryScreen()
    {
        StartCoroutine(LoadScreen());
    }

    IEnumerator LoadScreen()
    {
        // Play animation
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        gameManager.Victory();
    }
}
