using NPG.Codebase.Infrastructure.AutoSaving;
using NPG.Codebase.Infrastructure.GameBase.StateMachine;
using NPG.Codebase.Infrastructure.GameBase.StateMachine.GameStates;
using NPG.Codebase.Infrastructure.Services;
using UnityEngine;
using Zenject;

using PrefabProvider = NPG.Codebase.Infrastructure.Services.PrefabProviding.PrefabProvider;

namespace NPG.Codebase.Infrastructure.GameBase
{
    public class GameBootstrapper : MonoBehaviour
    {
        private GameStateMachine _stateMachine;
        private HubState _hubState;
        private SceneLoader _sceneLoader;
        private DiContainer _diContainer;

        [Inject]
        public void Construct(GameStateMachine stateMachine, HubState hubState ,SceneLoader sceneLoader, DiContainer diContainer)
        {
            _stateMachine = stateMachine;
            _hubState = hubState;
            _sceneLoader = sceneLoader;
            _diContainer = diContainer;
        }

        private void Start()
        {
            var prefab = PrefabProvider.LoadPrefab("AutoSaveController");
            _diContainer.InstantiatePrefab(prefab);
            
            _stateMachine.AddState(new BootstrapState(_sceneLoader, _stateMachine));
            _stateMachine.AddState(_hubState);
            _stateMachine.Enter<BootstrapState>();
        }
    }
}