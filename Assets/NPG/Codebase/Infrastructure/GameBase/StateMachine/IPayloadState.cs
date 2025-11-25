namespace NPG.Codebase.Infrastructure.GameBase.StateMachine
{
	public interface IPayloadState<T> :IExitableState
	{
		void Enter(T payload);
	}
}
