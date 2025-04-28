using System;
using System.Collections.Generic;
using System.Threading;
using Codebase.Game.Data;
using Codebase.Game.Modules;
using Codebase.Game.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Codebase.Game.Weapon
{
    public class RifleGun : Weapon
    {
        private void Update()
        {
            Debug.DrawLine(_muzzlePoints[0].position, AccuracyCorrection(weaponData.weaponSettings.accuracy, PlayerController.ToMousePosition(_muzzlePoints[0].position)) * 100f, Color.red);
        }

        public override async void Shoot(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (ShootingCts != null)
                    ShootingCts.Cancel();

                ShootingCts = new CancellationTokenSource();
                var token = ShootingCts.Token;

                float fireRateInSeconds = 60f / weaponData.weaponSettings.fireRate;
                IsShooting = true;

                while (IsShooting && !token.IsCancellationRequested)
                {
                    if (!IsReloading && CurrentAmmo > 0)
                    {
                        CastBullet(_muzzlePoints);
                    }
                    else if (CurrentAmmo <= 0)
                    {
                        Reload();
                        IsShooting = false;
                        break;
                    }

                    await UniTask.WaitForSeconds(fireRateInSeconds, cancellationToken: token);
                }
            }

            if (context.canceled)
            {
                IsShooting = false;
                ShootingCts?.Cancel();
            }
        }

        private void CastBullet(List<Transform> muzzlePoints)
        {
            Transform point = muzzlePoints[Random.Range(0, muzzlePoints.Count)];

            Vector3 origin = point.position;

            if (CurrentAmmo <= 0)
            {
                Debug.Log("No ammo, reloading...");
                Reload();
                return;
            }

            CurrentAmmo--;
            Debug.Log($"Current ammo: {CurrentAmmo}");
            
            bool hit = Physics.Raycast(origin, AccuracyCorrection(weaponData.weaponSettings.accuracy, PlayerController.ToMousePosition(origin)), out RaycastHit hitInfo);
    
            if (hit)
            {
                Debug.Log($"Hit: {hitInfo.collider.name}");
                
                if (hitInfo.collider.TryGetComponent(out HealthModule healthModule))
                {
                    _ = healthModule.ReceiveDamage(weaponData.weaponSettings.damage);
                }
            }
        }
    }
}