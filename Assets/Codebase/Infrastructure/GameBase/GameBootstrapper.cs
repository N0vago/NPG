using Codebase.Infrastructure.GameBase.StateMachine;
using Codebase.Infrastructure.GameBase.StateMachine.GameStates;
using Codebase.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace Codebase.Infrastructure.GameBase
{
    public class GameBootstrapper : MonoBehaviour
    {
        private GameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;

        [Inject]
        public void Construct(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        private void Start()
        {
            _stateMachine.AddState(new BootstrapState(_sceneLoader, _stateMachine));
            _stateMachine.AddState(new HubState());
            _stateMachine.Enter<BootstrapState>();
        }
    }
}