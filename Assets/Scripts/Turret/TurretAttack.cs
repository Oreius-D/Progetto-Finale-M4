using UnityEngine;

// Base class for all turret attack types.
// Implemented as a ScriptableObject so different attack behaviors can be created as reusable assets and swapped without modifying the FSM.
public abstract class TurretAttack : ScriptableObject
{
    // Minimum time (in seconds) between consecutive shots.
    // The FSM or attack controller is responsible for enforcing this cooldown.
    [Min(0f)]
    public float cooldown = 0.8f;

    // Executes the attack behavior.
    // firePoint: the origin position of the attack (e.g., muzzle).
    // target: the current target transform to attack.
    // Concrete attack types (laser, projectile, burst, etc.) must implement this method.
    public abstract void Fire(Transform firePoint, Transform target);
}