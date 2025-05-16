using UnityEngine;

namespace Codebase.Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "Projectiles/Projectile", order = 1)]
    public class ProjectileData : ScriptableObject
    {
        [Header("Projectile Settings")]
        public LayerMask damageLayer;
        public float projectileSpeed;
        public float projectileLifetime;
        
    }
}