using System;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components.Item
{
    [Flags]
    public enum ItemType
    {
        None = 0,
        Pistol = 1 << 0,
        Rifle = 1 << 1,
        Shotgun = 1 << 2,
        GrenadeLauncher = 1 << 3,
        EnergyWeapon = 1 << 4,
        Armor = 1 << 5,
        Artefact = 1 << 6
    }
}