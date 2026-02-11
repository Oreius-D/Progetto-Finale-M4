using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphState : ITurretState
{
    float timer;

    public void Enter(TurretFSM fsm)
    {
        timer = fsm.TelegraphTime;
        fsm.LaserOn();
    }

    public void Tick(TurretFSM fsm)
    {
        fsm.AcquireOrLoseTarget();
        if (fsm.Target == null) { fsm.ChangeState(fsm.Idle); return; }

        Vector3 tp = fsm.Target.position;
        fsm.AimAt(tp);
        fsm.LaserUpdate(tp);

        // se perde mira o LOS, torna a tracking
        if (!fsm.HasLOS(tp) || !fsm.IsAimed(tp))
        {
            fsm.ChangeState(fsm.Track);
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0f) fsm.ChangeState(fsm.Fire);
    }

    public void Exit(TurretFSM fsm) => fsm.LaserOff();
}
