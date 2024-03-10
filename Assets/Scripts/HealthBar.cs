using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    public UnityEngine.UI.Slider healthSlider;
    public TMP_Text healthBarText;

    Damageable damageable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("No player found in scene! Make sure it has tag Player.");
        }

        damageable = player.GetComponent<Damageable>();

        LoadPlayerHealth();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Update health slider and text based on the loaded player's health
        UpdateHealthUI(damageable.Health, damageable.MaxHealth);
    }

    private void OnEnable()
    {
        damageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        damageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        // Update health slider and text when player's health changes
        UpdateHealthUI(newHealth, maxHealth);

        // Save player's health to PlayerPrefs or other save system
        SavePlayerHealth(newHealth);
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        // Update health slider and text
        healthSlider.value = CalculateSliderPercentage(currentHealth, maxHealth);
        healthBarText.text = currentHealth + " / " + maxHealth;
    }

    private void SavePlayerHealth(int health)
    {
        // Save player's health to PlayerPrefs or other save system
        PlayerPrefs.SetInt("PlayerHealth", health);
    }

    private void LoadPlayerHealth()
    {
        // Load player's health from PlayerPrefs or other save system
        if (PlayerPrefs.HasKey("PlayerHealth"))
        {
            int health = PlayerPrefs.GetInt("PlayerHealth");
            damageable.Health = health;
        }
    }

    public void ResetPlayerHealth()
    {
        // Reset player's health to full
        damageable.Health = damageable.MaxHealth;

        // Update health slider and text
        UpdateHealthUI(damageable.Health, damageable.MaxHealth);

        // Save player's health to PlayerPrefs
        SavePlayerHealth(damageable.Health);
    }
}
