using NPG.Codebase.Game.Gameplay.Weapon;
using NPG.Codebase.Infrastructure.JsonData;
using NPG.Codebase.Infrastructure.Services.DataSaving;
using UnityEngine;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerCharacter : MonoBehaviour, IDataWriter
    {
        [SerializeField] private GameObject weaponHolder;
        
        private WeaponBase _currentWeapon;
        
        private PlayerController _playerController;
        private ProgressDataHandler _progressDataHandler;

        [Inject]
        public void Construct(ProgressDataHandler progressDataHandler)
        {
            Debug.Log("Constructing PlayerCharacter");
            _progressDataHandler = progressDataHandler;
            _progressDataHandler.RegisterObserver(this);
        }

        public void SetWeapon(WeaponBase weapon)
        {
            
            //Rework this in future
            if (_currentWeapon != null)
            {
                Destroy(_currentWeapon.gameObject);
            }
            var instance = Instantiate(weapon, weaponHolder.transform);
            _currentWeapon = instance;
            _currentWeapon.SetController(_playerController);
        }

        public void Load(GameData data)
        {
           int userIndex = data.GetCurrentUserIndex();
			Vector3 position = new Vector3(
                data.userData[userIndex].playerData.xPos,
                data.userData[userIndex].playerData.yPos,
                data.userData[userIndex].playerData.zPos
            );
           
           transform.position = position;
        }

        public void Save(ref GameData data)
        {
            int userIndex = data.GetCurrentUserIndex();
            data.userData[userIndex].playerData.xPos = Mathf.Round(gameObject.transform.position.x);
            data.userData[userIndex].playerData.yPos = Mathf.Round(gameObject.transform.position.y);
            data.userData[userIndex].playerData.zPos = Mathf.Round(gameObject.transform.position.z);
        }
        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            _playerController.StartFireAction += () =>
            {
                if (_currentWeapon == null)
                {
                    return;
                }
                _ = _currentWeapon.PullTrigger();
            };
            _playerController.StopFireAction += () =>
            {
                if (_currentWeapon == null)
                {
                    return;
                }
                _currentWeapon.ReleaseTrigger();
            };
            _playerController.ReloadAction += () =>
            {
                if (_currentWeapon == null)
                {
                    return;
                }
                _ = _currentWeapon.Reload();
            };
        }
    }
}