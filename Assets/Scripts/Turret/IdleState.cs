// IdleState represents the default resting state of the turret.
// In this state, the turret is not tracking or firing.
// It simply waits until a valid target is detected.
public class IdleState : ITurretState
{
    // Called once when entering Idle state.
    // Ensure any visual telegraphing (like laser indicators) is turned off while the turret is inactive.
    public void Enter(TurretFSM fsm) => fsm.LaserOff();

    // Called every frame while in Idle state.
    public void Tick(TurretFSM fsm)
    {
        // Continuously check if a target has entered detection range.
        fsm.AcquireOrLoseTarget();

        // If a valid target is found, transition to Track state to begin aiming.
        if (fsm.Target != null)
            fsm.ChangeState(fsm.Track);
    }

    // Called once when exiting Idle state.
    // No cleanup required here.
    public void Exit(TurretFSM fsm) { }
}