using UnityEngine;

namespace NPG.Codebase.Infrastructure.ScriptableObjects.ProjectileSO
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/Projectiles/Projectile", order = 1)]
    public class ProjectileData : ScriptableObject
    {
        [Header("Projectile Settings")]
        public LayerMask damageLayer;
        public float projectileSpeed;
        public float projectileLifetime;
        
    }
}