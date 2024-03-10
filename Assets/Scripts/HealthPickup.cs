using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    // Amount restored
    public int healthRestore = 20;
    // Height of bobbing
    public float bobbingHeight = 0.1f; 
    // Speed of bobbing
    public float bobbingSpeed = 5f;

    private Vector3 originalPosition;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Save the original position of the pickup
        originalPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            bool wasHealed = damageable.Heal(healthRestore);
            
            if(wasHealed)
            {
                if(audioSource != null)
                {
                    AudioSource.PlayClipAtPoint(audioSource.clip, gameObject.transform.position, audioSource.volume);
                }

                // If healed successfully, destroy the pickup
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        // Perform bobbing motion
        BobbingMotion();
    }

    private void BobbingMotion()
    {
        // Calculate vertical position offset based on sine function
        float yOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;

        // Update the position of the pickup with the offset
        transform.position = originalPosition + new Vector3(0, yOffset, 0);
    }
}
