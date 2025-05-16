using UnityEngine;

namespace Codebase.Game.Modules.Damage
{
    public interface IExplosive
    {
        void Explode(float damage, float explosionRadius, LayerMask damageLayer);
        
    }
}