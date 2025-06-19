using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components
{
    [CreateAssetMenu(fileName = "ItemSettings", menuName = "Equipment/ItemSettings", order = 2)]
    public class ItemSettings : ScriptableObject
    {
        public Sprite itemIcon;
        public string itemId;
        
        public int itemOccupiedColumns = 1;
        public int itemOccupiedRows = 1;

        private const int ItemXSize = 50;
        private const int ItemYSize = 50;
        
        public Vector2 GetItemSize() => new(ItemXSize * itemOccupiedColumns, ItemYSize * itemOccupiedRows);
    }
}