using UnityEngine;

[CreateAssetMenu(menuName = "Turret/Attacks/Laser Hit (Raycast)")]
public class LaserHitAttack : TurretAttack
{
    public float damage = 10f;
    //public float cooldown = 0.8f;
    public float maxDistance = 60f;
    public LayerMask hitMask;

    public override void Fire(Transform firePoint, Transform target)
    {
        Vector3 origin = firePoint.position;

        // mira al centro del collider se possibile
        var col = target.GetComponentInChildren<Collider>();
        Vector3 aimPoint = col ? col.bounds.center : target.position;

        Vector3 dir = (aimPoint - origin).normalized;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, maxDistance, hitMask))
        {
            var player = hit.collider.GetComponentInParent<PlayerHealth>();
            if (player != null)
                player.TakeDamage(damage);
        }
    }
}

