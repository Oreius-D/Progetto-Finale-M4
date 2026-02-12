using UnityEngine;

// TrackState is the active aiming state.
// The turret has a target and continuously rotates to follow it.
// If the turret achieves both line of sight and sufficient aim accuracy, it transitions into Telegraph to warn before firing.
public class TrackState : ITurretState
{
    // Called once when entering Track state.
    // Tracking itself does not require the laser warning indicator.
    public void Enter(TurretFSM fsm) => fsm.LaserOff();

    // Called every frame while in Track state.
    public void Tick(TurretFSM fsm)
    {
        // Refresh target acquisition each frame.
        // Target might leave range, die, or become invalid.
        fsm.AcquireOrLoseTarget();

        // If we lost the target, go back to Idle.
        if (fsm.Target == null)
        {
            fsm.ChangeState(fsm.Idle);
            return;
        }

        // Cache target position to keep decisions consistent this frame.
        Vector3 tp = fsm.Target.position;

        // Rotate turret to face the target.
        fsm.AimAt(tp);

        // If we have clear line of sight and are aimed accurately enough, transition to Telegraph (the warning phase before firing).
        if (fsm.HasLOS(tp) && fsm.IsAimed(tp))
            fsm.ChangeState(fsm.Telegraph);
    }

    // Called once when exiting Track state.
    // No cleanup required.
    public void Exit(TurretFSM fsm) { }
}