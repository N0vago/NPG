using System.Collections.Generic;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components
{
    [CreateAssetMenu(fileName = "ItemsDataBase", menuName = "Equipment/ItemsDataBase", order = 1)]
    public class ItemDataBase : ScriptableObject
    {
        public List<ItemData> itemDatas;

        public ItemSetting TryGetItemSetting(string itemID)
        {
            foreach (var itemData in itemDatas)
            {
                if (itemData.itemID == itemID)
                {
                    return itemData.itemSetting;
                }

            }
            Debug.LogWarning($"Item with ID {itemID} not found in ItemDataBase.");
            return null;
        }
    }
}