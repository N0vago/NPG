using System.IO;
using Codebase.Infrastructure.Data;
using Codebase.Infrastructure.GameBase.StateMachine;
using Codebase.Infrastructure.GameBase.StateMachine.GameStates;
using Codebase.Infrastructure.Services;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Codebase.Infrastructure.GameBase
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