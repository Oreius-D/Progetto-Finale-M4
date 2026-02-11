using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public float maxHealth = 100f; // Maximum health of the player
    public float currentHealth; // Current health of the player

    public HealthBar healthBar; // Reference to the HealthBar script to update the health bar UI

    public PauseManager pauseManager; // Reference to the PauseManager script to handle pausing the game on player death

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to maximum health at the start of the game
        healthBar.SetMaxHealth(maxHealth); // Set the maximum health in the health bar UI
    }

    // This Update method is commented out, but it can be used for testing purposes to simulate taking damage when the space key is pressed
    // Update is called once per frame
    //void Update()
    //{
    //    //function to test health decrease. Take 20 damage when the space key is pressed
    //    //if (Input.GetKeyDown(KeyCode.Space))
    //    //{
    //    //    TakeDamage(20f); // Call the TakeDamage method with a damage value of 20 when the space key is pressed
    //    //}
    //}

    // Method to handle taking damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Decrease current health by the damage value
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // Ensure current health does not go below 0 or above max health
        healthBar.SetHealth(currentHealth); // Update the health bar UI to reflect the new current health value
        if (currentHealth <= 0f)
        {
           Die(); // Call the Die method if current health is 0 or less
        }
    }

    // Method to handle player death (currently empty, but can be expanded to include death logic such as playing a death animation, restarting the level, etc.)
    private void Die()
    {
        // Show death screen and pause the game
        pauseManager.SetPaused(true); // Call the SetPaused method from the PauseManager to pause the game
        pauseManager.lose(true); // Call the lose method from the PauseManager to show the lose screen

    }

    // Method to handle healing the player
    public void Heal(float healAmount)
    {
        currentHealth += healAmount; // Increase current health by the heal amount
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // Ensure current health does not go below 0 or above max health
        healthBar.SetHealth(currentHealth); // Update the health bar UI to reflect the new current health value
    }
}
