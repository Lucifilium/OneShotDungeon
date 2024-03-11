using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextDialogue : MonoBehaviour
{
    public float fadeDuration = 3f;
    public string[] dialogueLines;

    private TextMeshProUGUI dialogueText;

    void Start()
    {
        dialogueText = GetComponent<TextMeshProUGUI>();

        // Start displaying dialogue lines
        StartCoroutine(DisplayDialogue());
    }

    IEnumerator DisplayDialogue()
    {
        foreach (string line in dialogueLines)
        {
            // Set the current line of dialogue
            dialogueText.text = line; 

            // Fade in
            Color originalColor = dialogueText.color;
            float alpha = 0f;

            // Increase alpha over time
            while (alpha < 1f)
            {
                alpha += Time.deltaTime / fadeDuration; 
                dialogueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            // Wait for the line to display
            yield return new WaitForSeconds(fadeDuration); 

            // Fade out
            while (alpha > 0)
            {
                // Decrease alpha over time
                alpha -= Time.deltaTime / fadeDuration; 
                dialogueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            // Restore original color
            dialogueText.color = originalColor; 
        }

        // Disable the GameObject when all lines have been displayed
        gameObject.SetActive(false);
        
    }
}
