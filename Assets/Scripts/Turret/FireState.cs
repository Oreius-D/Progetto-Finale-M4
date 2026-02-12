using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FireState handles the actual firing attempt of the turret.
// It assumes the turret has already locked and tracked a valid target.
public class FireState : ITurretState
{
    // Called once when entering the Fire state.
    // No setup is required here because all preparation
    // (aiming, telegraphing, etc.) happens in previous states.
    public void Enter(TurretFSM fsm) { }

    // Called every frame while in Fire state.
    public void Tick(TurretFSM fsm)
    {
        // Re-check if we still have a valid target.
        // Target might have died, left range, or been destroyed.
        fsm.AcquireOrLoseTarget();

        // If no target is available anymore, immediately return to Idle state.
        if (fsm.Target == null)
        {
            fsm.ChangeState(fsm.Idle);
            return;
        }

        // Cache target position for clarity and consistency.
        Vector3 tp = fsm.Target.position;

        // Rotate turret toward the target.
        // FireState still ensures we are correctly aimed.
        fsm.AimAt(tp);

        // Rotate turret toward the target.
        // FireState still ensures we are correctly aimed.
        if (!fsm.HasLOS(tp))
        {
            fsm.ChangeState(fsm.Track);
            return;
        }

        // If weapon cooldown has not finished, go back to Track and wait.
        if (!fsm.CanFire)
        {
            fsm.ChangeState(fsm.Track);
            return;
        }

        // Attempt to fire.
        // TryFire internally handles cooldown timing and executes the Attack logic.
        fsm.TryFire();

        // After firing (or attempting to fire),
        // return to Track state to continuously update aim and re-evaluate firing conditions.
        fsm.ChangeState(fsm.Track);
    }

    // Called once when exiting Fire state.
    // No cleanup required.
    public void Exit(TurretFSM fsm) { }
}