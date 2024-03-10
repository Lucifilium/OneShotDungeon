using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRemoveBehaviour : StateMachineBehaviour
{
    public float fadeTime = 1.5f;
    private float timeElapsed = 0f;

    SpriteRenderer spriteRenderer;
    GameObject objToRemove;
    Color startColor;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset the elapsed time to 0
        timeElapsed = 0f;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();

        // Store the initial color of the SpriteRenderer
        startColor = spriteRenderer.color;

        // Store a reference to the Animator's game object
        objToRemove = animator.gameObject;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed += Time.deltaTime;

        // Calculate the new alpha value based on the elapsed time and fade time
        float newAlpha = startColor.a * (1 - (timeElapsed / fadeTime));

        // Update the color of the SpriteRenderer with the new alpha value
        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

        // If the elapsed time exceeds the fade time, destroy the object
        if (timeElapsed > fadeTime)
        {
            Destroy(objToRemove);
        }
    }
}
