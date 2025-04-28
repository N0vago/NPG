using System;
using Codebase.Game.Weapon;
using Codebase.Infrastructure.Data;
using Codebase.Infrastructure.Services.DataSaving;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Codebase.Game.Player
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
            _progressDataHandler = progressDataHandler;
            _progressDataHandler.RegisterObserver(this);
        }

        public void SetWeapon(IWeapon weapon)
        {
            _currentWeapon = weapon;
        }

        public void Load(GameData data)
        {
            
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
            _playerController.OnFire += Fire;
            _playerController.OnReload += Reload;
        }

        private void OnDisable()
        {
            _playerController.OnFire -= Fire;
            _playerController.OnReload -= Reload;
            _progressDataHandler.SaveProgress(this);
        }

        private void Start()
        {
            SetWeapon(GetComponentInChildren<Weapon.Weapon>());
        }

        private void Fire(InputAction.CallbackContext context) => _currentWeapon.Shoot(context);

        private void Reload(InputAction.CallbackContext context) => _currentWeapon.Reload();
    }
}