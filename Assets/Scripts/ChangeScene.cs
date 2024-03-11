using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public float changeTime;
    public string sceneName;
    public KeyCode skipKey = KeyCode.Space;

    // Update is called once per frame
    void Update()
    {
        changeTime -= Time.deltaTime;
        // Check if the skip button is pressed
        if (Input.GetKeyDown(skipKey))
        {
            // Skip to the scene immediately
            SceneManager.LoadScene(sceneName);
        }

        if (changeTime <= 0)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
