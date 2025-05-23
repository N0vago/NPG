using NPG.Codebase.Game.Gameplay.Modules.Health;
using NPG.Codebase.Game.Gameplay.ScriptableObjects.WeaponSO;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.Weapon.WeaponSctipts
{
    public class Flamethrower : WeaponBase
    {

        protected override void CreateBullet()
        {
            if(weaponData is FlamethrowerData flamethrowerData)
            {
                Vector2 spread = CalculateShotSpread((int)flamethrowerData.spreadRadius);
                
                Vector3 toMousePos = PlayerController.MousePosition - muzzlePoint.position;
                
                Vector3 castDirection = new Vector3(toMousePos.x + spread.x, toMousePos.y, toMousePos.z + spread.y);
                
                
                bool isCollided = Physics.SphereCast(
                    muzzlePoint.position,
                    flamethrowerData.initialRadius,
                    castDirection.normalized,
                    out RaycastHit hit,
                    flamethrowerData.distance);
                
                
                Debug.DrawRay(muzzlePoint.position, castDirection.normalized * flamethrowerData.distance, Color.red, 1.0f);

                Vector3 endPoint = muzzlePoint.position + castDirection.normalized * flamethrowerData.distance;
                DebugExtension.DebugWireSphere(muzzlePoint.position, Color.yellow, flamethrowerData.initialRadius, 1f);
                DebugExtension.DebugWireSphere(endPoint, Color.cyan, flamethrowerData.initialRadius, 1f);
                
                if (isCollided && hit.collider.TryGetComponent(out IDamageable health))
                {
                    health.TakeDamage(weaponData.damage);
                }
            }
            else
            {
                Debug.LogError("Flamethrower data is not set correctly.");
            }
        }
        
        private Vector2 CalculateShotSpread(int spreadRadius)
        {
            float theta = Random.value * 2 * Mathf.PI;

            float randomValue = Mathf.Sqrt(Random.Range(0, spreadRadius + 1));
            
            Vector2 newPos = new Vector2(randomValue * Mathf.Cos(theta),
                randomValue * Mathf.Sin(theta));
            return newPos;
        }
    }
}