using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components.Item;
using R3;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components.Slot
{
    public class InventorySlotViewModel : ViewModel
    {
        
        private readonly ReactiveProperty<ItemViewModel> _item = new();
        public bool IsAvailable => _item.Value == null;
        public virtual string SlotID { get; private set; }
        public ReadOnlyReactiveProperty<ItemViewModel> Item => _item;

        public InventorySlotViewModel(string slotID)
        {
            SlotID = slotID;
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
            _item.Dispose();
        }
        
    }
}