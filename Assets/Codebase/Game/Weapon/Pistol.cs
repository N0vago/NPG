using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Codebase.Game.Weapon
{
    [RequireComponent(typeof(Rigidbody))]
    public class Pistol : MonoBehaviour, IWeapon
    {
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private Transform[] muzzlePoints = new Transform[2];
        
        private RaycastHit _hitInfo;
        
        private void Start()
        {
            if (weaponData.weaponType != WeaponType.Gun)
            {
                Debug.LogError($"Wrong configuration was set for: {gameObject.name}");
            }
        }

        private void Update()
        {
            Debug.DrawRay(muzzlePoints[0].position, muzzlePoints[0].up * 100f, Color.red);
            Debug.DrawRay(muzzlePoints[1].position, muzzlePoints[1].up * 100f, Color.red);
        }

        public void Shoot()
        {
            Transform point = muzzlePoints[0];
            Vector3 origin = point.position;
            
            
            if (weaponData.twoHanded)
            {
                point = muzzlePoints[Random.Range(0, 2)];
                origin = point.position;
            }
            
            
            bool raycast = Physics.Raycast(origin, point.up, out _hitInfo ,weaponData.range);
            
            if (raycast)
            {
                Debug.Log($"Hitted: {_hitInfo.collider.name}");
            }
            
        }

        public void Reload()
        {
            
        }
    }
}