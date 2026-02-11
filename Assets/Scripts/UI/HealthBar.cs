using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; // Reference to the UI Slider component representing the health bar
    public Gradient healthGradient; // Reference to a Gradient to change the color of the health bar based on health percentage
    public Image fillImage; // Reference to the Image component that fills the health bar, used to change its color

    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health; // Set the maximum value of the health slider
        // Update the value of the health slider to reflect the current health
        healthSlider.value = health;

        fillImage.color = healthGradient.Evaluate(1f); // Set the color of the health bar to the color corresponding to full health in the gradient    }
    }

    // Method to update the health bar based on the current health value
    public void SetHealth(float health)
    {
        healthSlider.value = health; // Update the value of the health slider to reflect the current health

        fillImage.color = healthGradient.Evaluate(healthSlider.normalizedValue); // Set the color of the health bar based on the current health percentage using the gradient
    }
}
