using System;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item;
using NPG.Codebase.Game.Gameplay.Weapon;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components
{
    [Serializable]
    public class ItemSetting 
    {
        public ItemType itemType = ItemType.None;
        public WeaponBase weaponPrefab;
        
        public int itemOccupiedColumns = 1;
        public int itemOccupiedRows = 1;
        
        public const int ItemXSize = 50;
        public const int ItemYSize = 50;
        
        public Vector2 GetItemSize() => new(ItemXSize * itemOccupiedColumns, ItemYSize * itemOccupiedRows);
    }
}