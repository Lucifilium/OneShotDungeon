using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public Canvas gameCanvas;

    // Subscribe to character damage and heal events
    private void OnEnable()
    {
        CharacterEvents.characterDamaged += (CharacterTookDamage);
        CharacterEvents.characterHealed += (CharacterHealed);
    }

    // Unsubscribe from character damage and heal events
    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= (CharacterTookDamage);
        CharacterEvents.characterHealed -= (CharacterHealed);
    }

    // Called when a character takes damage
    public void CharacterTookDamage(GameObject character, int damageRecieved)
    {
        // Create text at character hit
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        // Instantiate health text prefab at the calculated screen position.
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        // Set the text content to the damage received
        tmpText.text = damageRecieved.ToString();
    }

    // Called when character is healed
    public void CharacterHealed(GameObject character, int healthRestored)
    {
        // Create text at character heal
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        // Instantiate health text prefab at the calculated screen position
        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        // Set the text content to the health restored.
        tmpText.text = healthRestored.ToString();
    }
}
