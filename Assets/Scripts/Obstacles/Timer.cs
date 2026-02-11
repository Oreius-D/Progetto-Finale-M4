using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component that will display the timer
    private float timeRemaining = 300f; // Total time for the timer in seconds

    [SerializeField] private PauseManager pauseManager; // Reference to the PauseManager script to handle pausing the game when time runs out

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Decrease the remaining time by the time elapsed since the last frame
            UpdateTimerText(); // Update the timer text to reflect the new remaining time
        }
        else
        {
            timeRemaining = 0; // Ensure that the remaining time does not go below 0
            UpdateTimerText(); // Update the timer text to show that time has run out
            // You can add additional logic here for when the timer runs out, such as ending the game or triggering an event
        }
        if (timeRemaining <= 0)
        {
            TimeUp(); // Call the TimeUp method when the timer reaches 0 or less
        }
    }

    // Method to update the timer text
    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60); // Calculate the number of minutes remaining
        int seconds = Mathf.FloorToInt(timeRemaining % 60); // Calculate the number of seconds remaining
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Format the timer text as MM:SS
    }

    // Method to add time to the timer (e.g., when collecting a time orb)
    public void AddTime(float timeToAdd)
    {
        timeRemaining += timeToAdd; // Increase the remaining time by the specified amount
        UpdateTimerText(); // Update the timer text to reflect the new remaining time
    }

    //Display lose screen when time runs out
    private void TimeUp()
    {
        // Show lose screen and pause the game
        pauseManager.SetPaused(true); // Call the SetPaused method from the PauseManager to pause the game
        pauseManager.lose(true); // Call the lose method from the PauseManager to show the lose screen
    }
}
