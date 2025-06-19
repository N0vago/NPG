using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item;
using R3;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot
{
    public class InventorySlotViewModel : ViewModel
    {
        private readonly ReactiveProperty<ItemViewModel> _item = new();
        
        public bool IsAvailable => _item.Value == null;
        public ReadOnlyReactiveProperty<ItemViewModel> Item => _item;
        public RectTransform SlotRectTransform { get; set; }
        public InventorySlotViewModel()
        {
            Debug.Log("InventorySlotViewModel initialized with default values.");
        }
        public void LockSlot(ItemViewModel item)
        {
            if (item == null) return;
            _item.Value = item;
        }

        public void UnlockSlot(ItemViewModel item)
        {
            if (item == null || _item.Value != item) return;
            _item.Value = null;
        }
        
        public override void Dispose()
        {
            base.Dispose();

        }
    }
}