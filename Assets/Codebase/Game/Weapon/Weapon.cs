using System;
using System.Collections.Generic;
using System.Threading;
using Codebase.Game.Data;
using Codebase.Game.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Codebase.Game.Weapon
{
    public abstract class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] protected WeaponData weaponData;
        [SerializeField] protected WeaponType weaponType;
        [SerializeField] protected List<Transform> _muzzlePoints;
        [SerializeField] protected int reservedAmmo;

        //Currently here for prototyping purpose
        protected PlayerController PlayerController;
        
        protected CancellationTokenSource ShootingCts;

        protected bool IsEquiped;
        protected bool IsReloading;
        protected bool IsShooting;
        
        protected int CurrentAmmo;
        
        private void Start()
        {
            if (weaponData.weaponSettings.weaponType != weaponType)
            {
                Debug.LogError($"Wrong configuration was set for: {gameObject.name}");
            }

            PlayerController = GetComponentInParent<PlayerController>();
        }
        public abstract void Shoot(InputAction.CallbackContext context);

        public async void Reload()
        {
            if (IsReloading) return;

            IsReloading = true;

            await UniTask.WaitForSeconds(weaponData.weaponSettings.reloadTime);

            int neededAmmo = weaponData.weaponSettings.magSize - CurrentAmmo;

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
        
    }
}