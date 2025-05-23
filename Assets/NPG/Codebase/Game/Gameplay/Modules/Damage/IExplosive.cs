using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.Modules.Damage
{
    public interface IExplosive
    {
        void Explode(float damage, float explosionRadius, LayerMask damageLayer);
        
    }
}