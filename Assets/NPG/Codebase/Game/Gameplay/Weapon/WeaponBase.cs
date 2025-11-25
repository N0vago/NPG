using System.Threading;
using Cysharp.Threading.Tasks;
using NPG.Codebase.Game.Gameplay.Player;
using NPG.Codebase.Infrastructure.ScriptableObjects.WeaponSO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NPG.Codebase.Game.Gameplay.Weapon
{
    public abstract class WeaponBase : MonoBehaviour, IWeapon
    {
        [SerializeField] protected WeaponData weaponData;
        [SerializeField] protected Transform muzzlePoint;
        [SerializeField] protected int reservedAmmo;
        
        protected int CurrentAmmo;

        protected PlayerController PlayerController;

        private CancellationTokenSource _shootingCts;

        public bool IsReloading { get; private set; }

        public bool IsShooting { get; private set; }
        
        public void SetController(PlayerController playerController)
        {
            if (playerController == null)
            {
                Debug.LogError("PlayerController is null. Cannot set controller for weapon.");
                return;
            }
            PlayerController = playerController;
        }

        public async UniTask PullTrigger()
        {
            if (_shootingCts != null)
                _shootingCts.Cancel();
            
            _shootingCts = new CancellationTokenSource();
            var token = _shootingCts.Token;
            
            float fireRateInSeconds = 60f / weaponData.fireRate;
            IsShooting = true;

            while (IsShooting && !token.IsCancellationRequested)
            {
                if (!IsReloading && CurrentAmmo > 0)
                {
                    CreateBullet();
                }
                else if (CurrentAmmo <= 0)
                {
                    Reload().Forget();
                    IsShooting = false;
                    break;
                }

                await UniTask.WaitForSeconds(fireRateInSeconds, cancellationToken: token);
            }
        }

        public void ReleaseTrigger()
        {
            IsShooting = false;
            _shootingCts?.Cancel();
        }

        public async UniTask Reload()
        {
            if (IsReloading) return;

            IsReloading = true;

            await UniTask.WaitForSeconds(weaponData.reloadTime, cancellationToken: this.GetCancellationTokenOnDestroy());

            int neededAmmo = weaponData.magSize - CurrentAmmo;

            if (neededAmmo <= 0)
            {
                Debug.Log("Magazine already full");
            }
            else if (reservedAmmo > 0)
            {
                int ammoToLoad = Mathf.Min(neededAmmo, reservedAmmo);

                CurrentAmmo += ammoToLoad;
                reservedAmmo -= ammoToLoad;

                Debug.Log($"Reloaded {ammoToLoad} bullets. Current: {CurrentAmmo}, Reserved: {reservedAmmo}");
            }
            else
            {
                Debug.Log("Out of ammo");
            }

            IsReloading = false;
        }
        
        protected Vector3 AccuracyCorrection(int accuracy, Vector3 rawDirection)
        {
            float x = Random.Range(0f, accuracy);
            float y = Random.Range(0f, accuracy);
            Vector3 directionModifier = new Vector3(x, 0f, y);
            return rawDirection + directionModifier;
        }
        protected abstract void CreateBullet();
    }
}