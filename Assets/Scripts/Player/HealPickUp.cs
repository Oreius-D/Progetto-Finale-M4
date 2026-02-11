using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealPickUp : MonoBehaviour
{
    [SerializeField] private int healAmount = 50;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"ENTER -> {other.name} | tag={other.tag} | layer={other.gameObject.layer}");

        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player collided with HealPickUp");
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
            //Debug.Log($"PlayerHealth component found: {playerHealth != null}");

            if (playerHealth != null)
            {
                //Debug.Log($"Healing player for {healAmount} health");
                playerHealth.Heal(healAmount);
                //Debug.Log("Heal applied to player, deactivating HealPickUp");
                gameObject.SetActive(false);
                //Debug.Log("HealPickUp deactivated");
            }
        }
    }
}


