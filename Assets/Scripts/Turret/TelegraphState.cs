using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TelegraphState is the "warning" phase before firing.
// The turret shows a visible cue (laser) and requires the target to remain in line of sight AND properly aimed for a short duration.
// If conditions are broken, it falls back to Track.
public class TelegraphState : ITurretState
{
    // Countdown timer for the telegraph duration.
    // When it reaches zero, we transition to Fire.
    float timer;

    // Called once when entering Telegraph state.
    public void Enter(TurretFSM fsm)
    {
        // Reset the countdown to the configured telegraph duration.
        timer = fsm.TelegraphTime;

        // Enable the laser / warning indicator.
        fsm.LaserOn();
    }

    // Called every frame while in Telegraph state.
    public void Tick(TurretFSM fsm)
    {
        // Continuously refresh target acquisition.
        // Target might leave range or be destroyed during telegraphing.
        fsm.AcquireOrLoseTarget();

        // If we lost the target, return to Idle immediately.
        if (fsm.Target == null)
        {
            fsm.ChangeState(fsm.Idle);
            return;
        }

        // Cache target position for consistency this frame.
        Vector3 tp = fsm.Target.position;

        // Keep aiming during telegraphing to maintain accuracy.
        fsm.AimAt(tp);

        // Update laser endpoint to match current target position.
        // This makes the telegraph visually track the player.
        fsm.LaserUpdate(tp);

        // If we lose line of sight OR we are no longer sufficiently aimed, stop telegraphing and go back to Track to reacquire aim/LOS.
        if (!fsm.HasLOS(tp) || !fsm.IsAimed(tp))
        {
            fsm.ChangeState(fsm.Track);
            return;
        }

        // Decrease telegraph timer.
        timer -= Time.deltaTime;

        // Once telegraph time is completed, move to Fire state.
        if (timer <= 0f)
            fsm.ChangeState(fsm.Fire);
    }

    // Called once when exiting Telegraph state.
    // Ensure the warning indicator is disabled.
    public void Exit(TurretFSM fsm) => fsm.LaserOff();
}