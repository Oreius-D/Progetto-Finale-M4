using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class TurretFSM : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform yawPivot;     // Base
    [SerializeField] private Transform pitchPivot;   // Head o Cylinder
    [SerializeField] private Transform firePoint;    // Where the projectile spawns
    [SerializeField] private LineRenderer aimLaser;
    [SerializeField] private TurretAttack attack;    // ScriptableObject

    [Header("Detection")]
    [SerializeField] private float aggroRange = 10f;
    [SerializeField] private float loseRange = 12f;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Aim")]
    [SerializeField] private float yawSpeed = 180f;
    [SerializeField] private float pitchSpeed = 180f;
    [SerializeField] private float pitchMin = -20f;
    [SerializeField] private float pitchMax = 45f;
    [SerializeField] private float aimToleranceDeg = 2f;
    [SerializeField] private bool usePitch = true;

    [Header("Timing")]
    [SerializeField] private float telegraphTime = 0.6f;
    [SerializeField] private float nextAllowedFireTime;

    [Header("Detection")]
    [SerializeField] private Transform detectionOrigin;

    // current state and transition method
    public Transform Target { get; private set; }
    ITurretState current;

    
    public readonly IdleState Idle = new();
    public readonly TrackState Track = new();
    public readonly TelegraphState Telegraph = new();
    public readonly FireState Fire = new();

    void Awake()
    {
        if (aimLaser != null)
        {
            aimLaser.positionCount = 2;
            aimLaser.enabled = false;
        }
        ChangeState(Idle);
    }

    void Update()
    {
        current?.Tick(this);
        //Debug.Log("TurretFSM Update");
    }

    public void ChangeState(ITurretState next)
    {
        current?.Exit(this);
        current = next;
        current?.Enter(this);
    }

    // ===== Shared Helpers =====

    // ===== Public API (clean) =====
    public Transform FirePoint => firePoint;
    public TurretAttack Attack => attack;
    public float TelegraphTime => telegraphTime;

    public bool CanFire => Time.time >= nextAllowedFireTime;

    public bool TryFire()
    {
        if (attack == null || firePoint == null || Target == null) return false;

        float cd = attack.cooldown > 0f ? attack.cooldown : 0.8f;

        if (Time.time < nextAllowedFireTime) return false;

        nextAllowedFireTime = Time.time + cd;
        attack.Fire(firePoint, Target);
        return true;
    }

    public void AcquireOrLoseTarget()
    {
        Vector3 origin = detectionOrigin != null ? detectionOrigin.position : transform.position;

        if (Target == null)
        {
            Collider[] hits = Physics.OverlapSphere(origin, aggroRange, playerMask);
            if (hits.Length > 0) Target = hits[0].transform;
        }
        else
        {
            float d = Vector3.Distance(origin, Target.position);
            if (d > loseRange) Target = null;
        }
    }

    public void AimAt(Vector3 targetPos)
    {
        // YAW
        Vector3 toTarget = targetPos - yawPivot.position;
        Vector3 flat = new Vector3(toTarget.x, 0f, toTarget.z);
        if (flat.sqrMagnitude > 0.0001f)
        {
            var yawRot = Quaternion.LookRotation(flat, Vector3.up);
            yawPivot.rotation = Quaternion.RotateTowards(yawPivot.rotation, yawRot, yawSpeed * Time.deltaTime);
        }

        if (!usePitch)
        {
            pitchPivot.localRotation = Quaternion.RotateTowards(
                pitchPivot.localRotation,
                Quaternion.identity,
                pitchSpeed * Time.deltaTime
            );
            return;
        }

        // PITCH (local compared to yawPivot)
        Vector3 localDir = yawPivot.InverseTransformDirection((targetPos - pitchPivot.position).normalized);
        float pitch = -Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        var desired = Quaternion.Euler(pitch, 0f, 0f);
        pitchPivot.localRotation = Quaternion.RotateTowards(pitchPivot.localRotation, desired, pitchSpeed * Time.deltaTime);
    }

    public bool IsAimed(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - firePoint.position).normalized;
        return Vector3.Angle(firePoint.forward, dir) <= aimToleranceDeg;
    }

    public bool HasLOS(Vector3 targetPos)
    {
        Vector3 origin = firePoint.position;
        Vector3 dir = targetPos - origin;
        float dist = dir.magnitude;
        dir /= dist;

        // if obstacle hit before player -> no LOS
        return !Physics.Raycast(origin, dir, dist, obstacleMask);
    }

    public void LaserOn()
    {
        if (aimLaser == null) return;
        aimLaser.enabled = true;
    }

    public void LaserOff()
    {
        if (aimLaser == null) return;
        aimLaser.enabled = false;
    }

    public void LaserUpdate(Vector3 targetPos)
    {
        if (aimLaser == null || !aimLaser.enabled) return;

        aimLaser.SetPosition(0, firePoint.position);

        Vector3 dir = (targetPos - firePoint.position).normalized;
        float maxDist = 100f;

        if (Physics.Raycast(firePoint.position, dir, out var hit, maxDist, obstacleMask | playerMask))
            aimLaser.SetPosition(1, hit.point);
        else
            aimLaser.SetPosition(1, firePoint.position + dir * maxDist);
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Vector3 origin = detectionOrigin != null ? detectionOrigin.position : transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, aggroRange);
        Gizmos.color = new Color(1f, 0.5f, 0f);
        Gizmos.DrawWireSphere(origin, loseRange);
    }
#endif
}