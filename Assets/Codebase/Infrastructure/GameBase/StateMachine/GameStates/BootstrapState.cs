using Codebase.Infrastructure.Services;

namespace Codebase.Infrastructure.GameBase.StateMachine.GameStates
{
    public class BootstrapState : IState
    {
        private GameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;
        
        public BootstrapState(SceneLoader sceneLoader, GameStateMachine stateMachine)
        {
            _sceneLoader = sceneLoader;
            _stateMachine = stateMachine;
        }
        public async void Enter()
        {
            await _sceneLoader.LoadSceneAsync((int)SceneIDs.Hub);
            
            _stateMachine.Enter<HubState>();
        }

        public void Exit()
        {
            
        }
    }
}