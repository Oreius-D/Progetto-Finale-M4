using System.Collections;
using UnityEngine;

public class LavaTickDmg : MonoBehaviour
{
    // This script applies damage over time to players who enter the lava trigger zone.
    [Header("Damage Settings")]
    [SerializeField] private float damageAmount = 1f;
    [SerializeField] private float tickInterval = 0.5f;

    // Coroutine reference to manage the damage over time process
    private Coroutine dmgRoutine;

    private void OnTriggerEnter(Collider other)
    {
        // Only start the damage routine if the collider belongs to a player
        if (!other.CompareTag("Player")) return;

        // Start the damage over time routine if it's not already running
        if (dmgRoutine == null)
            dmgRoutine = StartCoroutine(DamageOverTime(other));
    }

    private void OnTriggerExit(Collider other)
    {
        // Only stop the damage routine if the collider belongs to a player
        if (!other.CompareTag("Player")) return;

        // Stop the damage routine when the player exits the lava trigger zone
        if (dmgRoutine != null)
        {
            StopCoroutine(dmgRoutine); // Stop the ongoing damage routine
            dmgRoutine = null; // Clear the reference to allow starting a new routine if the player re-enters
        }
    }

    // Coroutine that applies damage to the player at regular intervals while they are in the lava trigger zone
    private IEnumerator DamageOverTime(Collider playerCollider)
    {
        // Get the PlayerHealth component from the player collider's parent object
        var playerHealth = playerCollider.GetComponentInParent<PlayerHealth>();

        // Continuously apply damage at regular intervals as long as the player is still in the lava trigger zone
        while (playerHealth != null)
        {
            // Apply damage to the player
            playerHealth.TakeDamage(damageAmount);
            // Wait for the specified tick interval before applying damage again
            yield return new WaitForSeconds(tickInterval);
        }

        // Clear the coroutine reference when the player is no longer valid (e.g., if they die or leave the trigger zone)
        dmgRoutine = null;
    }
}
