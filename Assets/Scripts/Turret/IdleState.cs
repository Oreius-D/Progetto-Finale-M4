public class IdleState : ITurretState
{
    public void Enter(TurretFSM fsm) => fsm.LaserOff();
    public void Tick(TurretFSM fsm)
    {
        fsm.AcquireOrLoseTarget();
        if (fsm.Target != null) fsm.ChangeState(fsm.Track);
    }
    public void Exit(TurretFSM fsm) { }
}
