﻿using NPG.Codebase.Game.Gameplay.Weapon;
using NPG.Codebase.Infrastructure.JsonData;
using NPG.Codebase.Infrastructure.Services.DataSaving;
using UnityEngine;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerCharacter : MonoBehaviour, IDataWriter
    {
        private IWeapon _currentWeapon;
        
        private PlayerController _playerController;
        private ProgressDataHandler _progressDataHandler;

        [Inject]
        public void Construct(ProgressDataHandler progressDataHandler)
        {
            Debug.Log("Constructing PlayerCharacter");
            _progressDataHandler = progressDataHandler;
            _progressDataHandler.RegisterObserver(this);
        }

        public void SetWeapon(IWeapon weapon)
        {
            _currentWeapon = weapon;
        }

        public void Load(GameData data)
        {
            transform.position = data.playerData.playerPosition;
        }

        public void Save(ref GameData data)
        {
            data.playerData.playerPosition.x = Mathf.Round(gameObject.transform.position.x);
            data.playerData.playerPosition.y = Mathf.Round(gameObject.transform.position.y);
            data.playerData.playerPosition.z = Mathf.Round(gameObject.transform.position.z);
        }
        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            _playerController.StartFireAction += () => _currentWeapon.PullTrigger();
            _playerController.StopFireAction += () => _currentWeapon.ReleaseTrigger();
            _playerController.ReloadAction += () => _currentWeapon.Reload();
        }
        

        private void OnDisable()
        {
            _progressDataHandler.SaveProgress(this);
        }

        private void Start()
        {
            SetWeapon(GetComponentInChildren<WeaponBase>());
        }
    }
}