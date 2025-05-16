using System;
using Codebase.Game.Modules;
using Codebase.Game.Modules.Damage;
using Codebase.Infrastructure.Services.ObjectPooling;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Codebase.Game.Weapon.Projectile
{
    [RequireComponent(typeof(Rigidbody))]
    public class GrenadeProjectile : MonoBehaviour, IPoolable<GrenadeProjectile>, IExplosive
    {

        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody => _rigidbody;

        public event Action<Collision> OnCollision;
        public event Action<GrenadeProjectile> OnReturnAction;

        public void Initialize(Action<GrenadeProjectile> returnAction) => OnReturnAction = returnAction;

        public void ReturnToPool()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            
            OnReturnAction?.Invoke(this);
            OnCollision = null;
        }

        public void Explode(float damage, float explosionRadius, LayerMask damageLayer = default)
        {

            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            
            DrawDebugSphere(transform.position, explosionRadius);

            foreach (Collider nearbyObject in colliders)
            {
                IDamageable damageable = nearbyObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    float distance = Vector3.Distance(transform.position, nearbyObject.transform.position);
                    float finalDamage = damage * (1 - distance / explosionRadius);
                    damageable.TakeDamage(finalDamage);
                }
            }
            ReturnToPool();
        }

        public void CreateExplosionEffect(GameObject explosionEffect)
        {
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            OnCollision?.Invoke(other);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        public void DrawDebugSphere(Vector3 center, float radius, int segments = 36, Color? color = null)
        {
            Color sphereColor = color ?? Color.red;
            float angleStep = 360f / segments;

            for (int i = 0; i < segments; i++)
            {
                float angle1 = Mathf.Deg2Rad * (i * angleStep);
                float angle2 = Mathf.Deg2Rad * ((i + 1) * angleStep);

                // Draw circle in the XZ plane
                Vector3 point1 = center + new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * radius;
                Vector3 point2 = center + new Vector3(Mathf.Cos(angle2), 0, Mathf.Sin(angle2)) * radius;

                Debug.DrawLine(point1, point2, sphereColor, 2f); // Duration of 2 seconds
            }
        }
        
    }
}