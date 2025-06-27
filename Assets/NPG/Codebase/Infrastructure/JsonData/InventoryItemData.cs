using System;
using System.Collections.Generic;

namespace NPG.Codebase.Infrastructure.JsonData
{
    [Serializable]
    public class InventoryItemData
    {
        public List<string> ItemIDs = new List<string>(); 
        
        public string ContainerID = string.Empty;
    }
}