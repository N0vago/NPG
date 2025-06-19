using System;
using System.Collections.Generic;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item;
using NPG.Codebase.Infrastructure.IDs;
using ObservableCollections;
using R3;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot
{
    public class SlotContainerViewModel : ViewModel
    {
        private readonly ReactiveProperty<Grid2D<InventorySlotViewModel>> _slotsGrid;

        private readonly ObservableList<InventorySlotViewModel> _slots = new();
        public ReadOnlyReactiveProperty<Grid2D<InventorySlotViewModel>> SlotsGrid => _slotsGrid;
        public IObservableCollection<InventorySlotViewModel> Slots => _slots;
        public virtual string ContainedSlotsID { get; } = SlotIDs.DefaultSlot;
        
        public SlotContainerViewModel(Grid2D<InventorySlotViewModel> slotsGrid, Func<InventorySlotViewModel> defaultSlot)
        {
            _slotsGrid = new ReactiveProperty<Grid2D<InventorySlotViewModel>>(slotsGrid);
            
            _slotsGrid.Value.InitializeGrid(defaultSlot, _slots.Add);
        }
        
        public List<InventorySlotViewModel> GetOccupiedSlotsForItem(ItemViewModel item)
        {
            List<InventorySlotViewModel> occupiedSlots = new List<InventorySlotViewModel>();
            
            foreach (var slot in _slots)
            {
                if (slot.Item.CurrentValue == item)
                {
                    occupiedSlots.Add(slot);
                }
            }
            return occupiedSlots;
        }
        
        //TODO: No problem if new row added but if new column added it can be placed in wrong position on the view
        public void ExpandGrid(int row, int column)
        {
            _slotsGrid.Value.ExpandGrid(row, column, _slotsGrid.Value[0, 0], true, _slots.Add);
        }
    }
}