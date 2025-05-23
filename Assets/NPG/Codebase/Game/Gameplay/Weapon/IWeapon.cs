using Cysharp.Threading.Tasks;

namespace NPG.Codebase.Game.Gameplay.Weapon
{
    public interface IWeapon
    {
        UniTask PullTrigger();
        
        void ReleaseTrigger();

        UniTask Reload();
    }
}