using Codebase.Game.Modules;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Codebase.Game.Weapon.WeaponSctipts
{
    public class Shotgun : Weapon
    {
        private const int BuckshotCount = 10;
        protected override void CreateBullet()
        {
            if (CurrentAmmo <= 0)
            {
                Reload().Forget();
                return;
            }

            CurrentAmmo--;
            Debug.Log($"Current ammo: {CurrentAmmo}");
            
            Vector3 origin = muzzlePoint.position;
            
            for (int i = 0; i <= BuckshotCount; i++)
            {
                Vector3 toMousePos = PlayerController.MousePosition - muzzlePoint.position;
                Vector2 pos = CalculateBuckshotPos(weaponData.accuracy);
                Vector3 newPos = new Vector3(toMousePos.x + pos.x, toMousePos.y, toMousePos.z + pos.y);
                
                var hit = Physics.Raycast(origin, 
                    newPos,
                            out RaycastHit hitInfo
                );

                if (hit)
                {
                    Debug.Log($"Hit: {hitInfo.collider.name}");
                    Debug.DrawLine(
                        origin, 
                        newPos * 100f,
                        Color.red,
                        1f
                    );
                    if (hitInfo.collider.TryGetComponent(out IDamageable damageable))
                    {
                        _ = damageable.TakeDamage(weaponData.damage);
                    }
                }
            }
        }

        private Vector2 CalculateBuckshotPos(int spreadRadius)
        {
            float theta = Random.value * 2 * Mathf.PI;

            float randomValue = Mathf.Sqrt(Random.Range(0, spreadRadius + 1));
            
            Vector2 newPos = new Vector2(randomValue * Mathf.Cos(theta),
                randomValue * Mathf.Sin(theta));
            return newPos;
        }
    }
}