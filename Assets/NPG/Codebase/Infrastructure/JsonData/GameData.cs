using System;
using System.Collections.Generic;

namespace NPG.Codebase.Infrastructure.JsonData
{
    [Serializable]
    public class GameData
    {
        public PlayerData playerData = new();

        public List<InventoryItemData> inventoryItemData = new();
    }
}