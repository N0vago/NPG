using System;
using System.Collections;
using Codebase.Game.Player;
using Codebase.Infrastructure.Data;
using Codebase.Infrastructure.Services.DataSaving;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

using PrefabProvider = Codebase.Infrastructure.Services.PrefabProviding.PrefabProvider;

namespace Codebase.Infrastructure.Services
{
    public class HubInitializer : MonoBehaviour, IDataReader
    {
        [Tooltip("The addressable prefabs to instantiate" +
                 " in the hub. Note that order witch you add them" +
                 " in the inspector will be the order they are instantiated" +
                 " in the hub.")]
        [SerializeField] private AssetReference[] hubPrefabs;
        
        private Transform _playerTransform;
        private PlayerController _playerController;
        private CinemachineCamera _cinemachineCamera;
        
        private ProgressDataHandler _progressData;

        private Vector3 _playerPosition = new();

        [Inject]
        public void Construct(ProgressDataHandler progressDataHandler)
        {
            _progressData = progressDataHandler;
            _progressData.RegisterObserver(this);
            InitHub();
        }
        
        private void InitHub()
        {
            for(int i = 0; i < hubPrefabs.Length; i++)
            {
                var prefab = PrefabProvider.GetPrefab(hubPrefabs[i]);
                var instance = Instantiate(prefab);
                
                if (prefab.TryGetComponent(out PlayerController _))
                {
                    _playerController = instance.GetComponent<PlayerController>();
                    _playerTransform = instance.gameObject.transform;
                }
                if(prefab.TryGetComponent(out CinemachineCamera _))
                {
                    _cinemachineCamera = instance.GetComponent<CinemachineCamera>();;
                }
            }
            
            _playerController.transform.position = _playerPosition;

            StartCoroutine(DelayedInitialize());
        }
        private IEnumerator DelayedInitialize()
        {
            yield return null;
            if (_cinemachineCamera?.Target != null)
            {
                _cinemachineCamera.Target.TrackingTarget = _playerController.gameObject.transform;
            }
            else
            {
                Debug.LogError("CinemachineCamera or Target is not initialized properly.");
            }
        }
        public void Load(GameData data)
        {
            _playerPosition = data.playerData.playerPosition;
        }
    }
}