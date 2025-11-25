using System;
using System.Collections.Generic;

namespace NPG.Codebase.Infrastructure.JsonData
{
    [Serializable]
    public class PlayerData
    {
		public float xPos;
        public float yPos;
        public float zPos;

		public List<InventoryItemData> inventoryItemData = new();
		
		public PlayerStatistics playerStatistics = new();
	}
}