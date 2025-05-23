using System;
using Cysharp.Threading.Tasks;
using NPG.Codebase.Game.Gameplay.ScriptableObjects.ProjectileSO;
using NPG.Codebase.Game.Gameplay.Weapon.Projectile;
using NPG.Codebase.Infrastructure.Services.ObjectPooling;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.Weapon.WeaponSctipts
{
    public class GrenadeLauncher : WeaponBase
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