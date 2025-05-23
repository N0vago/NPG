namespace NPG.Codebase.Infrastructure.GameBase.StateMachine
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}