using NPG.Codebase.Infrastructure.GameBase.StateMachine;
using NPG.Codebase.Infrastructure.GameBase.StateMachine.GameStates;
using NPG.Codebase.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace NPG.Codebase.Infrastructure.GameBase
{
    public class GameBootstrapper : MonoBehaviour
    {
        private GameStateMachine _stateMachine;
        private HubState _hubState;
        private SceneLoader _sceneLoader;

        [Inject]
        public void Construct(GameStateMachine stateMachine, HubState hubState ,SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _hubState = hubState;
            _sceneLoader = sceneLoader;
        }

        private void Start()
        {
            _stateMachine.AddState(new BootstrapState(_sceneLoader, _stateMachine));
            _stateMachine.AddState(_hubState);
            _stateMachine.Enter<BootstrapState>();
        }
    }
}