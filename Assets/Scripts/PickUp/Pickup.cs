using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private PickupEffect effect;

    private void OnTriggerEnter(Collider other)
    {
        if (effect == null) return;

        if (effect.Apply(other.gameObject))
            gameObject.SetActive(false);
    }
}

