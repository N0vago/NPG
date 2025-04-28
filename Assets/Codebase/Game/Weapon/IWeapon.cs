using UnityEngine.InputSystem;

namespace Codebase.Game.Weapon
{
    public interface IWeapon
    {
        void Shoot(InputAction.CallbackContext context);

        void Reload();
    }
}