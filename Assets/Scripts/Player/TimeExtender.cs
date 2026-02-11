using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeExtender : MonoBehaviour
{
    // variable to get the timer to extend
    public TextMeshProUGUI timeText;
    [SerializeField] private Timer timer; // timer reference to add time to when collecting a time orb

    private void OnTriggerEnter(Collider other)
    {
        // if the player collides with a coin
        if (other.CompareTag("TimeOrb"))
        {
            // Add 30 seconds to the timer when the player collects a time orb
            timer.AddTime(30f);

            // deactivate the time orb object instead of destroying it to allow for respawning later
            gameObject.SetActive(false);
        }
    }
}
