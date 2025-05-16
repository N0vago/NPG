using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Codebase.Game.Weapon
{
    public interface IWeapon
    {
        UniTask PullTrigger();
        
        void ReleaseTrigger();

        UniTask Reload();
    }
}