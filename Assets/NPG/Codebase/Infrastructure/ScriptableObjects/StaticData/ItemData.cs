using System;
using NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components;

namespace NPG.Codebase.Infrastructure.ScriptableObjects.StaticData
{
    [Serializable]
    public class ItemData
    {
        public string itemID = string.Empty;
        public ItemSetting itemSetting;
    }
}