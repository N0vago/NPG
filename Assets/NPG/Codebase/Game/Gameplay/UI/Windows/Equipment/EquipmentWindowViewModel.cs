using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot;
using NPG.Codebase.Infrastructure.IDs;
using R3;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment
{
    public class EquipmentWindowViewModel : WindowViewModel
    {
        private readonly ReactiveProperty<SlotContainerViewModel> _slotContainer;
        
        private readonly ReactiveProperty<ItemContainerViewModel> _itemContainerViewModel;
        public ReadOnlyReactiveProperty<SlotContainerViewModel> SlotContainer => _slotContainer;
        public ReadOnlyReactiveProperty<ItemContainerViewModel> ItemContainerViewModel => _itemContainerViewModel;
        public override string Id { get; } = WindowIDs.EquipmentWindow;
        
        public EquipmentWindowViewModel()
        {
            _slotContainer = new ReactiveProperty<SlotContainerViewModel>(
                new SlotContainerViewModel(new Grid2D<InventorySlotViewModel>(12, 6, 50), 
                    () => new InventorySlotViewModel()));
            _itemContainerViewModel = new ReactiveProperty<ItemContainerViewModel>(new ItemContainerViewModel(_slotContainer.Value));
            
            Debug.Log("EquipmentWindowViewModel initialized with default SlotContainer and ItemContainer.");
        }

        public override void Dispose()
        {
            base.Dispose();
            _slotContainer?.Dispose();
            _itemContainerViewModel?.Dispose();
        }
    }
}