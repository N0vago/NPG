﻿using Cysharp.Threading.Tasks;
using NPG.Codebase.Game.Gameplay.Modules.Health;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.Weapon.WeaponSctipts
{
    public class Pistol : WeaponBase
    {
        
        protected override void CreateBullet()
        {
            if (CurrentAmmo <= 0)
            {
                Debug.Log("No ammo, reloading...");
                Reload().Forget();
                return;
            }

            CurrentAmmo--;
            Debug.Log($"Current ammo: {CurrentAmmo}");
            
            Vector3 origin = muzzlePoint.position;
            
            Vector3 newPos = AccuracyCorrection(weaponData.accuracy,
                PlayerController.MousePosition - muzzlePoint.position);
            bool hit = Physics.Raycast(origin,newPos , out RaycastHit hitInfo);
            
            
            Debug.DrawLine(
                origin, 
                newPos * 100f,
                Color.red,
                1f
            );
            
            if (hit)
            {
                Debug.Log($"Hit: {hitInfo.collider.name}");
                
                if (hitInfo.collider.TryGetComponent(out IDamageable damageable))
                {
                    _ = damageable.TakeDamage(weaponData.damage);
                }
            }
        }

        
    }
}