using System.Collections.Generic;
using System.Threading;
using Codebase.Game.Modules;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Codebase.Game.Weapon
{
    public class Shotgun : Weapon
    {
        private const int BuckshotCount = 10;

        private void Update()
        {
            for (int i = 0; i <= 10; i++)
            {
                Vector2 pos = CalculateBuckshotPos(
                    weaponData.weaponSettings.spreadRadius,
                    PlayerController.ToMousePosition(_muzzlePoints[0].position).normalized
                );
                Debug.DrawLine(
                    _muzzlePoints[0].position, 
                     new Vector3(pos.x, 0, pos.y) * 100f,
                    Color.red
                );
            }
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
                Reload();
                return;
            }

            CurrentAmmo--;

           
            for (int i = 0; i <= BuckshotCount; i++)
            {
                
                var hit = Physics.Raycast(origin, 
                    CalculateBuckshotPos(weaponData.weaponSettings.spreadRadius, PlayerController.ToMousePosition(origin)),
                            out RaycastHit hitInfo
                );

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

        private Vector2 CalculateBuckshotPos(int spreadRadius, Vector3 origin)
        {
            float theta = Random.value * 2 * Mathf.PI;

            float randomValue = Mathf.Sqrt(Random.Range(0, spreadRadius + 1));

            return new Vector2(origin.x + randomValue * Mathf.Cos(theta), origin.z + randomValue * Mathf.Sin(theta));
        }
    }
}