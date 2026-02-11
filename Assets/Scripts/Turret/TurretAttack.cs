using UnityEngine;

public abstract class TurretAttack : ScriptableObject
{
    [Min(0f)]
    public float cooldown = 0.8f;

    public abstract void Fire(Transform firePoint, Transform target);
}

