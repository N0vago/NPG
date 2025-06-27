using System.Collections.Generic;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot;
using NPG.Codebase.Infrastructure.IDs;
using R3;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment
{
    public class EquipmentWindowViewModel : WindowViewModel
    {
        private readonly Dictionary<(ItemType, int), ReactiveProperty<SlotContainerViewModel>> _slotContainers = new();
        private readonly Dictionary<(ItemType, int), ReactiveProperty<ItemContainerViewModel>> _itemContainers = new();
        
        private readonly Dictionary<string, ItemContainerViewModel> _containerById = new();

        public override string Id => WindowIDs.EquipmentWindow;

        public EquipmentWindowViewModel()
        {
            InitContainer(ItemType.None, 0, "InventoryContainer", SlotIDs.DefaultSlot, 12, 6);
            InitContainer(ItemType.Armor, 0, "ArmorContainer", SlotIDs.ArmorSlot, 2, 2);
            
            InitContainer(ItemType.Pistol, 0, "PistolContainer", SlotIDs.PistolSlot, 1, 1);
            InitContainer(ItemType.Shotgun, 0, "ShotgunContainer", SlotIDs.ShotgunSlot, 3, 1);
            InitContainer(ItemType.Rifle, 0, "RifleContainer", SlotIDs.RifleSlot, 3, 1);
            InitContainer(ItemType.GrenadeLauncher, 0, "GrenadeLauncherContainer", SlotIDs.GrenadeLauncherSlot, 4, 1);
            InitContainer(ItemType.EnergyWeapon, 0, "EnergyGunContainer", SlotIDs.EnergyGunSlot, 4, 1);
            
            for (int i = 0; i < 5; i++)
            {
                string containerId = $"ArtefactContainer{i + 1}";
                string slotId = "ArtefactSlot";
                InitContainer(ItemType.Artefact, i, containerId, slotId, 1, 1);
            }
        }

        private void InitContainer(ItemType type, int index, string containerId, string slotId, int cols, int rows)
        {
            var slotContainer = new ReactiveProperty<SlotContainerViewModel>(
                new SlotContainerViewModel(
                    new Grid2D<InventorySlotViewModel>(cols, rows, 50),
                    () => new InventorySlotViewModel(slotId)));

            var itemContainer = new ReactiveProperty<ItemContainerViewModel>(
                new ItemContainerViewModel(slotContainer.Value, containerId));

            var key = (type, index);
            _slotContainers[key] = slotContainer;
            _itemContainers[key] = itemContainer;
            
            _containerById[containerId] = itemContainer.Value;

        }
        
        public ReadOnlyReactiveProperty<SlotContainerViewModel> GetSlotContainer(ItemType type)
            => GetSlotContainer((type, 0));

        public ReadOnlyReactiveProperty<ItemContainerViewModel> GetItemContainer(ItemType type)
            => GetItemContainer((type, 0));
        
        public ReadOnlyReactiveProperty<SlotContainerViewModel> GetSlotContainer((ItemType, int) key)
            => _slotContainers.TryGetValue(key, out var container) ? container : null;

        public ReadOnlyReactiveProperty<ItemContainerViewModel> GetItemContainer((ItemType, int) key)
            => _itemContainers.TryGetValue(key, out var container) ? container : null;
        
        public ItemContainerViewModel GetItemContainer(string containerId) 
            => _containerById.TryGetValue(containerId, out var container) ? container : null;
        
        public List<ItemContainerViewModel> GetAllItemContainers()
        {
            var containers = new List<ItemContainerViewModel>();
            foreach (var itemContainer in _itemContainers.Values)
            {
                if (itemContainer.Value != null)
                {
                    containers.Add(itemContainer.Value);
                }
            }
            return containers;
        }
        public override void Dispose()
        {
            base.Dispose();

            foreach (var disposable in _slotContainers.Values)
                disposable?.Dispose();

            foreach (var disposable in _itemContainers.Values)
                disposable?.Dispose();
        }
    }
}