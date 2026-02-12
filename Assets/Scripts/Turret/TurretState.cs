// ITurretState defines the contract for a turret FSM state.
// Each state is responsible for its own behavior and can request transitions via fsm.ChangeState(...).
//
// Lifecycle:
// - Enter(...) is called once when the state becomes active
// - Tick(...) is called every frame while the state is active
// - Exit(...) is called once when the state is replaced by another state
public interface ITurretState
{
    // Called once when the FSM switches into this state.
    // Use this to initialize timers, enable visuals, reset variables, etc.
    void Enter(TurretFSM fsm);

    // Called every frame while this state is active.
    // This is the main per-frame logic for the state.
    void Tick(TurretFSM fsm);

    // Called once when the FSM switches out of this state.
    // Use this to clean up (disable visuals, stop effects, etc.).
    void Exit(TurretFSM fsm);
}