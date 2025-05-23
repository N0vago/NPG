using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.ScriptableObjects.WeaponSO
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/Weapon", order = 1)]
    public class WeaponData : ScriptableObject
    {
        [Header("Basic Info")]
        public string weaponName;
        public Sprite icon;
        [Header("Stats")]
        public float damage;
        public float reloadTime;
        public float fireRate;
        public int magSize;
        [Range(0, 4)]
        public int accuracy;
        [Header("FX")]
        public AudioClip fireSound;
        public ParticleSystem muzzleFlash;
    }
}