using UnityEngine;

// LaserHitAttack is a hitscan (raycast-based) turret attack.
// It instantly checks for a hit along a straight line and applies damage if a valid PlayerHealth component is detected.
[CreateAssetMenu(menuName = "Turret/Attacks/Laser Hit (Raycast)")]
public class LaserHitAttack : TurretAttack
{
    // Damage applied on successful hit.
    public float damage = 10f;

    // Maximum raycast distance.
    public float maxDistance = 60f;

    // Layers considered valid for raycast collision.
    // Should typically include player and possibly obstacles.
    public LayerMask hitMask;

    // Executes the hitscan attack.
    public override void Fire(Transform firePoint, Transform target)
    {
        // Ray origin at muzzle position.
        Vector3 origin = firePoint.position;

        // Try to aim at the center of the target's collider for better visual/physical alignment.
        var col = target.GetComponentInChildren<Collider>();
        Vector3 aimPoint = col ? col.bounds.center : target.position;

        // Compute normalized direction toward aim point.
        Vector3 dir = (aimPoint - origin).normalized;

        // Perform raycast up to maxDistance against specified layers.
        if (Physics.Raycast(origin, dir, out RaycastHit hit, maxDistance, hitMask))
        {
            // Look for PlayerHealth on the hit object or its parent.
            // This allows colliders on child objects to still work.
            var player = hit.collider.GetComponentInParent<PlayerHealth>();

            if (player != null)
                player.TakeDamage(damage);
        }
    }
}
