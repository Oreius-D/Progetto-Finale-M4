using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTickDmg : MonoBehaviour
{
    public float damageAmount = 1f;
    public float tickInterval = 0.5f;

    private Coroutine dmgRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (dmgRoutine == null)
            dmgRoutine = StartCoroutine(DamageOverTime(other));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (dmgRoutine != null)
        {
            StopCoroutine(dmgRoutine);
            dmgRoutine = null;
        }
    }

    private IEnumerator DamageOverTime(Collider playerCollider)
    {
        var playerHealth = playerCollider.GetComponentInParent<PlayerHealth>();

        while (playerHealth != null) // finché esiste / non è stato distrutto
        {
            playerHealth.TakeDamage(damageAmount);
            yield return new WaitForSeconds(tickInterval);
        }

        dmgRoutine = null;
    }
}
