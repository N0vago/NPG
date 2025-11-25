using System.Collections.Generic;
using NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components;
using NPG.Codebase.Infrastructure.ScriptableObjects.StaticData;
using UnityEngine;

namespace NPG.Codebase.Infrastructure.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ItemsDataBase", menuName = "ScriptableObjects/Equipment/ItemsDataBase", order = 1)]
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