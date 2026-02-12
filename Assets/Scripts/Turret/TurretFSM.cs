using UnityEngine;

// TurretFSM is the main controller for the turret finite state machine.
// Responsibilities:
// - Holds references (pivots, firepoint, laser, attack asset)
// - Exposes shared helper methods used by all states (targeting, aiming, LOS, firing, laser visuals)
// - Runs the current state Tick() each frame and handles state transitions
public class TurretFSM : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform yawPivot;     // Rotates around Y (base)
    [SerializeField] private Transform pitchPivot;   // Rotates around X (head/cannon)
    [SerializeField] private Transform firePoint;    // Where shots originate (muzzle)
    [SerializeField] private LineRenderer aimLaser;  // Visual telegraph / aim indicator
    [SerializeField] private TurretAttack attack;    // ScriptableObject defining the attack behavior

    [Header("Detection")]
    [SerializeField] private float aggroRange = 10f;     // Acquire target within this range
    [SerializeField] private float loseRange = 12f;      // Drop target beyond this range (hysteresis)
    [SerializeField] private LayerMask playerMask;       // Which layers count as player targets
    [SerializeField] private LayerMask obstacleMask;     // Which layers block LOS / laser raycasts

    [Header("Aim")]
    [SerializeField] private float yawSpeed = 180f;      // Degrees per second for yaw rotation
    [SerializeField] private float pitchSpeed = 180f;    // Degrees per second for pitch rotation
    [SerializeField] private float pitchMin = -20f;      // Pitch clamp (down)
    [SerializeField] private float pitchMax = 45f;       // Pitch clamp (up)
    [SerializeField] private float aimToleranceDeg = 2f; // Allowed angular error to be "aimed"
    [SerializeField] private bool usePitch = true;       // If false, turret stays flat (no pitch)

    [Header("Timing")]
    [SerializeField] private float telegraphTime = 0.6f;     // How long warning phase lasts before firing
    [SerializeField] private float nextAllowedFireTime;      // Internal timestamp for cooldown gating

    [Header("OriginPoints")]
    [SerializeField] private Transform detectionOrigin;      // Optional origin for target acquisition checks

    // ===== FSM Runtime State =====
    // Current target chosen by the turret (null if none).
    public Transform Target { get; private set; }

    // Active state instance.
    ITurretState current;

    // Concrete states (kept as single instances).
    public readonly IdleState Idle = new();
    public readonly TrackState Track = new();
    public readonly TelegraphState Telegraph = new();
    public readonly FireState Fire = new();

    void Awake()
    {
        // Ensure the laser renderer is initialized and starts disabled.
        if (aimLaser != null)
        {
            // Set up the LineRenderer to have 2 points (start and end) and be disabled by default.
            aimLaser.positionCount = 2;
            aimLaser.enabled = false;
        }

        // Start in Idle by default.
        ChangeState(Idle);
    }

    void Update()
    {
        // Run the current state's per-frame logic.
        current?.Tick(this);
    }

    // Switches the FSM to a new state, calling Exit/Enter hooks.
    public void ChangeState(ITurretState next)
    {
        current?.Exit(this);
        current = next;
        current?.Enter(this);
    }

    // ===== Public API for states (clean surface) =====
    // Expose key references / parameters without giving full write access.
    public Transform FirePoint => firePoint;
    public TurretAttack Attack => attack;
    public float TelegraphTime => telegraphTime;

    // True if global cooldown has finished.
    public bool CanFire => Time.time >= nextAllowedFireTime;

    // Attempts to fire using the current TurretAttack asset.
    // Handles cooldown scheduling internally and returns whether a shot was fired.
    public bool TryFire()
    {
        // Defensive checks: attack setup must exist and target must be valid.
        if (attack == null || firePoint == null || Target == null) return false;

        // Use attack-defined cooldown, fallback to a safe default.
        float cd = attack.cooldown > 0f ? attack.cooldown : 0.8f;

        // Cooldown gate.
        if (Time.time < nextAllowedFireTime) return false;

        // Schedule next allowed shot time.
        nextAllowedFireTime = Time.time + cd;

        // Execute attack behavior.
        attack.Fire(firePoint, Target);
        return true;
    }

    // Acquire a new target if none, or drop current target if too far.
    // Uses aggro/lose ranges to avoid rapid target flickering.
    public void AcquireOrLoseTarget()
    {
        // Use explicit detection origin if provided; otherwise fallback to turret position.
        Vector3 origin = detectionOrigin != null ? detectionOrigin.position : transform.position;

        if (Target == null)
        {
            // Acquire first player collider found in range.
            // If multiple players/colliders will exist, change this to "closest target" logic.
            Collider[] hits = Physics.OverlapSphere(origin, aggroRange, playerMask);
            if (hits.Length > 0) Target = hits[0].transform;
        }
        else
        {
            // Drop target if it leaves the lose range.
            float d = Vector3.Distance(origin, Target.position);
            if (d > loseRange) Target = null;
        }
    }

    // Aims turret toward a world position using yaw (global) and optional pitch (local).
    public void AimAt(Vector3 targetPos)
    {
        // ----- YAW (rotate base toward target on the horizontal plane) -----
        Vector3 toTarget = targetPos - yawPivot.position;
        Vector3 flat = new Vector3(toTarget.x, 0f, toTarget.z);

        // Only rotate if we have a meaningful direction vector.
        if (flat.sqrMagnitude > 0.0001f)
        {
            // Desired yaw rotation to look at the target on the horizontal plane.
            Quaternion yawRot = Quaternion.LookRotation(flat, Vector3.up);
            // Smoothly rotate yaw pivot toward the desired rotation at the configured speed.
            yawPivot.rotation = Quaternion.RotateTowards(
                yawPivot.rotation,
                yawRot,
                yawSpeed * Time.deltaTime
            );
        }

        // If pitch is disabled, smoothly return pitch pivot to neutral.
        if (!usePitch)
        {
            // Smoothly rotate pitch pivot back to identity (no pitch) at the configured speed.
            pitchPivot.localRotation = Quaternion.RotateTowards(
                pitchPivot.localRotation,
                Quaternion.identity,
                pitchSpeed * Time.deltaTime
            );
            return;
        }

        // ----- PITCH (rotate head up/down relative to yaw pivot) -----
        // Convert direction into yawPivot local space so pitch is consistent with yaw rotation.
        Vector3 localDir = yawPivot.InverseTransformDirection((targetPos - pitchPivot.position).normalized);

        // Compute pitch angle (negative because Unity pitch usually goes "down" with positive X rotation).
        float pitch = -Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;

        // Clamp pitch to avoid unrealistic rotations.
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // Desired local rotation for the pitch pivot to look at the target.
        Quaternion desired = Quaternion.Euler(pitch, 0f, 0f);
        // Smoothly rotate pitch pivot toward the desired rotation at the configured speed.
        pitchPivot.localRotation = Quaternion.RotateTowards(
            pitchPivot.localRotation,
            desired,
            pitchSpeed * Time.deltaTime
        );
    }

    // Returns true if the firePoint forward direction is within tolerance toward targetPos.
    public bool IsAimed(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - firePoint.position).normalized;
        return Vector3.Angle(firePoint.forward, dir) <= aimToleranceDeg;
    }

    // Returns true if there is no obstacle blocking the straight line between firePoint and targetPos.
    public bool HasLOS(Vector3 targetPos)
    {
        // Raycast from firePoint to targetPos, checking for obstacles.
        Vector3 origin = firePoint.position;
        // Direction and distance to target.
        Vector3 dir = targetPos - origin;
        // Calculate distance to target for raycast length.
        float dist = dir.magnitude;

        // Safety: avoid divide-by-zero if targetPos equals origin.
        if (dist <= 0.0001f) return true;

        // Normalize direction for raycast.
        dir /= dist;

        // If we hit an obstacle before reaching the target distance, LOS is blocked.
        return !Physics.Raycast(origin, dir, dist, obstacleMask);
    }

    // Enable laser visual.
    public void LaserOn()
    {
        // Safety check: ensure we have a LineRenderer reference before trying to enable it.
        if (aimLaser == null) return;
        // Enable the LineRenderer to show the laser/telegraph.
        aimLaser.enabled = true;
    }

    // Disable laser visual.
    public void LaserOff()
    {
        // Safety check: ensure we have a LineRenderer reference before trying to disable it.
        if (aimLaser == null) return;
        // Disable the LineRenderer to hide the laser/telegraph.
        aimLaser.enabled = false;
    }

    // Update laser line positions: start at firePoint, end at first hit (player/obstacle) or max distance.
    public void LaserUpdate(Vector3 targetPos)
    {
        // Only update if laser exists and is currently enabled.
        if (aimLaser == null || !aimLaser.enabled) return;

        // Start point at muzzle.
        aimLaser.SetPosition(0, firePoint.position);

        // Shoot a ray in the direction of the target for visuals (not necessarily gameplay).
        Vector3 dir = (targetPos - firePoint.position).normalized;
        float maxDist = 100f;

        // Raycast against obstacles OR player to stop the laser on the first surface hit.
        if (Physics.Raycast(firePoint.position, dir, out RaycastHit hit, maxDist, obstacleMask | playerMask))
            aimLaser.SetPosition(1, hit.point);
        else
            aimLaser.SetPosition(1, firePoint.position + dir * maxDist);
    }

#if UNITY_EDITOR
    // Draw detection radii in the editor for tuning.
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
