using System;
using Codebase.Game.Modules;
using Codebase.Game.ScriptableObjects;
using Codebase.Game.Weapon.Projectile;
using Codebase.Infrastructure.Services.ObjectPooling;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Codebase.Game.Weapon.WeaponMonoBehaviors
{
    public class GrenadeLauncher : Weapon
    {
        [SerializeField] private ExplosiveProjectileData projectileData;

        [SerializeField] private GrenadeProjectile projectilePrefab;

        [Tooltip("Transform that contain projectiles")] [SerializeField]
        private Transform magTransform;

        private ObjectPool<GrenadeProjectile> _objectPool;


        protected override async void CreateBullet()
        {
            if (CurrentAmmo <= 0)
            {
                Debug.Log("No ammo, reloading...");
                Reload().Forget();
                return;
            }

            CurrentAmmo--;
            Debug.Log($"Current ammo: {CurrentAmmo}");
            
            GrenadeProjectile projectile = _objectPool.Pull(muzzlePoint.position, Quaternion.identity);
            
            float distance = Vector3.Distance(muzzlePoint.position, PlayerController.MousePosition);
            
            Vector3 direction = (PlayerController.MousePosition - muzzlePoint.position).normalized;
            
            projectile.Rigidbody.AddForce(direction * projectileData.projectileSpeed * distance, ForceMode.Impulse);
            
            try
            {
                projectile.OnCollision += (collision) =>
                {
                    projectile.Explode(weaponData.damage, projectileData.explosionRadius);
                };
                
                await UniTask.Delay(TimeSpan.FromSeconds(projectileData.projectileLifetime),
                    cancellationToken: this.GetCancellationTokenOnDestroy());

                if (projectile.gameObject.activeInHierarchy)
                {
                    projectile.Explode(weaponData.damage, projectileData.explosionRadius);
                }
            }
            catch (OperationCanceledException)
            {

            }
        }
        private void Start()
        {
            _objectPool = new ObjectPool<GrenadeProjectile>(projectilePrefab, magTransform, weaponData.magSize);
        }
    }



}