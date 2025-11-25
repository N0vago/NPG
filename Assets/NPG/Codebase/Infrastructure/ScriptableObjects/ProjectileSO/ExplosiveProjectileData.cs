using UnityEngine;

namespace NPG.Codebase.Infrastructure.ScriptableObjects.ProjectileSO
{
    [CreateAssetMenu(fileName = "ExplosiveProjectileData", menuName = "ScriptableObjects/Projectiles/ExplosiveProjectile", order = 2)]
    public class ExplosiveProjectileData : ProjectileData
    {
        [Header("Explosion Settings")]
        public GameObject explosionEffect;
        public float explosionRadius;
    }
}