using UnityEngine;
using UnityEngine.Serialization;

namespace Codebase.Game.Weapon
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
    public class WeaponData : ScriptableObject
    {
        
        public WeaponType weaponType;
        //Pistol
        public bool twoHanded;
        //Shotgun
        public float spreadRadius;
        //Grenade launcher
        public float explosionRadius ;
        public float explosionDelay ;
        public float projectileSpeed;
        //Energy gun
        public float freezingTime;
        //Flamethrower
        public float sphereRadius;

        [Header("General settings")] 
        public GameObject weaponPrefab;
        
        public float damage;
        public float reloadTime;
        public int range;
        public float fireRate;
        public int magSize;
        public int currentAmmo;
        public int accuracy;

        [HideInInspector]
        public bool isReloading;
    }

    public enum WeaponType
    {
        Gun,
        Shotgun,
        MachineGun,
        GrenadeLauncher,
        Flamethrower,
        EnergyGun
    }
}