using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/Heal Effect")]
public class HealEffect : PickupEffect
{
    [SerializeField] private int amount = 50;

    public override bool Apply(GameObject receiver)
    {
        var healable = receiver.GetComponentInParent<IHealable>();
        if (healable == null) return false;

        healable.Heal(amount);
        return true;
    }
}

