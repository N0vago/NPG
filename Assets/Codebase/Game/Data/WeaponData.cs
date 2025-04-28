using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Codebase.Game.Data
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
    public class WeaponData : ScriptableObject
    {
        public WeaponSettings weaponSettings;
    }

    public enum WeaponType
    {
        Pistol,
        Shotgun,
        MachineGun,
        GrenadeLauncher,
        Flamethrower,
        EnergyGun
    }

    [Serializable]
    public struct WeaponSettings
    {
        public WeaponType weaponType;
        //Shotgun
        public int spreadRadius;
        //Grenade launcher
        public float explosionRadius ;
        public float explosionDelay ;
        public float projectileSpeed;
        //Energy gun
        public float freezingTime;
        //Flamethrower
        public float sphereRadius;
        
        public GameObject weaponPrefab;

        public float damage;
        public float reloadTime;
        public float fireRate;
        public int magSize;
        
        public int accuracy;
    }
}