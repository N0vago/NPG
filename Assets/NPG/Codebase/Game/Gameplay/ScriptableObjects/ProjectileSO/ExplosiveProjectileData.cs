using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.ScriptableObjects.ProjectileSO
{
    [CreateAssetMenu(fileName = "ExplosiveProjectileData", menuName = "Projectiles/ExplosiveProjectile", order = 2)]
    public class ExplosiveProjectileData : ProjectileData
    {
        [Header("Explosion Settings")]
        public GameObject explosionEffect;
        public float explosionRadius;
    }
}