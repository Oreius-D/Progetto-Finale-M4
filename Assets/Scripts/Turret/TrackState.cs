using UnityEngine;

public class TrackState : ITurretState
{
    public void Enter(TurretFSM fsm) => fsm.LaserOff();

    public void Tick(TurretFSM fsm)
    {
        fsm.AcquireOrLoseTarget();
        if (fsm.Target == null) { fsm.ChangeState(fsm.Idle); return; }

        Vector3 tp = fsm.Target.position;
        fsm.AimAt(tp);

        if (fsm.HasLOS(tp) && fsm.IsAimed(tp))
            fsm.ChangeState(fsm.Telegraph);
    }

    public void Exit(TurretFSM fsm) { }
}
