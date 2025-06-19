using System;
using System.Collections.Generic;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot;
using NPG.Codebase.Infrastructure.IDs;
using R3;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item
{
    public class ItemViewModel : ViewModel
    {
        private readonly Subject<List<InventorySlotBinder>> _selectedSlots = new();
        
        public Observable<List<InventorySlotBinder>> SelectedSlots => _selectedSlots;
        public virtual string ItemID { get; } = ItemIDs.DefaultItem;
        
        public void AddSelectedSlot(List<InventorySlotBinder> slots)
        {
            _selectedSlots.OnNext(slots);
        }
        
    }
}