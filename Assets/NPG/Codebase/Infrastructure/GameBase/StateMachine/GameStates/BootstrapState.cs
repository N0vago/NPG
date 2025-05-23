using NPG.Codebase.Infrastructure.IDs;
using NPG.Codebase.Infrastructure.Services;

namespace NPG.Codebase.Infrastructure.GameBase.StateMachine.GameStates
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
            await _sceneLoader.LoadSceneAsync((int)SceneIDs.Hub, OnSceneLoaded);
        }

        private void OnSceneLoaded()
        {
            _stateMachine.Enter<HubState>();
        }

        public void Exit()
        {
            
        }
    }
}