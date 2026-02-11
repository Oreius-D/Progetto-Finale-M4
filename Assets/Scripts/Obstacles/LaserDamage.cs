using System.Collections.Generic;
using UnityEngine;

public class LaserDamageTick : MonoBehaviour
{
    public float damage = 10f; // laser damage per tick
    public float tickSeconds = 0.25f; // seconds between damage ticks

    // Register the next tick time for each player currently in the laser
    private readonly Dictionary<GameObject, float> nextTick = new();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        nextTick[other.gameObject] = Time.time; // tick immediately on enter
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove the player from the nextTick dictionary when they exit the laser
        nextTick.Remove(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        // Only apply damage to objects tagged as "Player"
        if (!other.CompareTag("Player")) return;

        // Check if it's time to apply damage again for this player
        if (!nextTick.TryGetValue(other.gameObject, out float t)) return;
        if (Time.time < t) return;

        // Apply damage to the player
        var health = other.GetComponentInParent<PlayerHealth>();
        if (health != null) health.TakeDamage(damage);

        // Schedule the next tick for this player
        nextTick[other.gameObject] = Time.time + tickSeconds;
    }
}
