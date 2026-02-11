using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurretState
{
    void Enter(TurretFSM fsm);
    void Tick(TurretFSM fsm);
    void Exit(TurretFSM fsm);
}
