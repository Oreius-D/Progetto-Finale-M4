using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireState : ITurretState
{
    public void Enter(TurretFSM fsm) { }

    public void Tick(TurretFSM fsm)
    {
        fsm.AcquireOrLoseTarget();
        if (fsm.Target == null)
        {
            fsm.ChangeState(fsm.Idle);
            return;
        }

        Vector3 tp = fsm.Target.position;
        fsm.AimAt(tp);

        // NON sparare se non c'è LOS
        if (!fsm.HasLOS(tp))
        {
            fsm.ChangeState(fsm.Track);
            return;
        }

        // Se non posso sparare ancora, torno a Track
        if (!fsm.CanFire)
        {
            fsm.ChangeState(fsm.Track);
            return;
        }

        // Provo a sparare (gestisce cooldown + Fire internamente)
        fsm.TryFire();

        fsm.ChangeState(fsm.Track);
    }

    public void Exit(TurretFSM fsm) { }
}

