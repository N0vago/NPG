using UnityEngine;
using UnityEngine.Serialization;

namespace Codebase.Game.ScriptableObjects
{
    [CreateAssetMenu (fileName = "FlamethrowerData", menuName = "Weapons/Flamethrower", order = 2)]
    public class FlamethrowerData : WeaponData
    {
        [Header("Flamethrower Settings")]
        [Tooltip("The radius that determines cast sphere size")]
        public float initialRadius;
        [Tooltip("The radius that determines how flame spreads")]
        public float spreadRadius;
        [Tooltip("The distance of the flamethrower flame cast")]
        public float distance;
        
        
    }
}