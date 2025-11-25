using NPG.Codebase.Game.Gameplay.Achievements;
using NPG.Codebase.Game.Gameplay.Player;
using NPG.Codebase.Game.Gameplay.UI.Factories;
using NPG.Codebase.Game.Gameplay.UI.HUD;
using NPG.Codebase.Infrastructure.ScriptableObjects;
using NPG.Codebase.Infrastructure.ScriptableObjects.StaticData;
using NPG.Codebase.Infrastructure.Services.DataSaving;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;
using PrefabProvider = NPG.Codebase.Infrastructure.Services.PrefabProviding.PrefabProvider;

namespace NPG.Codebase.Infrastructure.GameBase.StateMachine.GameStates
{
    public class HubState : IState
    {
        private HubObjects _hubObjects;
        
        private PlayerController _playerController;
        private CinemachineCamera _cinemachineCamera;
        private UIRootFactory _uiRootFactory;

        private DiContainer _container;
        

        public void Exit()
        {
        }

        public void Enter()
        {
            _container = Object.FindObjectOfType<SceneContext>().Container;
            _hubObjects = _container.Resolve<HubObjects>();
            AchievementManager.Instance.FirstLaunch();
            InitHub();
        }
        
        private void InitHub()
        {
            foreach (var hubObject in _hubObjects.hubObjects)
            {
                GameObject prefab;
                GameObject instance;

                switch (hubObject.hubID)
                {
                    case HubIDs.Player:
                        prefab = PrefabProvider.LoadPrefab(hubObject.addressableName);
                        instance = _container.InstantiatePrefab(prefab);
                        _playerController = instance.GetComponent<PlayerController>();
                        break;
                    case HubIDs.Camera:
                        prefab = PrefabProvider.LoadPrefab(hubObject.addressableName);
                        instance = Object.Instantiate(prefab);
                        break;
                    case HubIDs.CinemachineCamera:
                        prefab = PrefabProvider.LoadPrefab(hubObject.addressableName);
                        instance = Object.Instantiate(prefab);
                        _cinemachineCamera = instance.GetComponent<CinemachineCamera>();
                        _cinemachineCamera.Target.TrackingTarget = _playerController.gameObject.transform;
                        break;
                    case HubIDs.UIRoot:
                        _uiRootFactory = _container.Resolve<UIRootFactory>();
                        _uiRootFactory.CreateUIRoot(hubObject.addressableName);
                        break;
                    case HubIDs.HUDCanvas:
                        prefab = PrefabProvider.LoadPrefab(hubObject.addressableName);
                        instance = _container.InstantiatePrefab(prefab, _uiRootFactory.UIRootBinder.transform);
                        HUDBinder hudBinder = instance.GetComponent<HUDBinder>();
                        _uiRootFactory.UIRootBinder.AttachScreenBinder(hudBinder);
						break;
				}
            }

            if (_cinemachineCamera != null)
            {
                if (_cinemachineCamera?.Target != null)
                {
                    _cinemachineCamera.Target.TrackingTarget = _playerController.gameObject.transform;
                }
                else
                {
                    Debug.LogError("CinemachineCamera or Target is not initialized properly.");
                }
            }
        }
    }
}