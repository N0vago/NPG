using System.Collections.Generic;
using System.Linq;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot;
using R3;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment
{
    public class EquipmentWindowBinder : WindowBinder
    {
        [Header("Inventory setups")]
        [SerializeField] SlotContainerBinder inventorySlotContainerBinder;
        [SerializeField] ItemContainerBinder inventoryItemContainerBinder;
        [SerializeField] SlotContainerBinder armorSlotContainerBinder;
        [SerializeField] ItemContainerBinder armorItemContainerBinder;
        [Header("Weapon setups")]
        [SerializeField] SlotContainerBinder[] weaponSlotContainerBinders;
        [SerializeField] ItemContainerBinder[] weaponItemContainerBinders;
        [Header("Artefact setups")]
        [SerializeField] SlotContainerBinder[] artefactSlotContainerBinders;
        [SerializeField] ItemContainerBinder[] artefactItemContainerBinders;
        
        private EquipmentWindowViewModel _equipmentWindowViewModel;
        
        
        protected override void OnBind(WindowViewModel viewModel)
        {
            base.OnBind(viewModel);

            if (viewModel is not EquipmentWindowViewModel equipmentWindowViewModel)
            {
                Debug.LogError("EquipmentWindowBinder: ViewModel is not of type EquipmentWindowViewModel");
                return;
            }

            _equipmentWindowViewModel = equipmentWindowViewModel;
            
            _disposables.Add(_equipmentWindowViewModel
                .GetItemContainer(ItemType.None)
                .Subscribe(inventoryItemContainerBinder.Bind));

            _disposables.Add(_equipmentWindowViewModel
                .GetSlotContainer(ItemType.None)
                .Subscribe(inventorySlotContainerBinder.Bind));
            
            _disposables.Add(_equipmentWindowViewModel
                .GetItemContainer(ItemType.Armor)
                .Subscribe(armorItemContainerBinder.Bind));

            _disposables.Add(_equipmentWindowViewModel
                .GetSlotContainer(ItemType.Armor)
                .Subscribe(armorSlotContainerBinder.Bind));
            
            var weaponTypes = new[]
            {
                ItemType.Pistol,
                ItemType.Shotgun,
                ItemType.Rifle,
                ItemType.GrenadeLauncher,
                ItemType.EnergyWeapon
            };

            for (int i = 0; i < weaponTypes.Length; i++)
            {
                var type = weaponTypes[i];

                var slot = _equipmentWindowViewModel.GetSlotContainer(type);
                var item = _equipmentWindowViewModel.GetItemContainer(type);

                if (slot != null && i < weaponSlotContainerBinders.Length)
                {
                    _disposables.Add(slot.Subscribe(weaponSlotContainerBinders[i].Bind));
                }

                if (item != null && i < weaponItemContainerBinders.Length)
                {
                    _disposables.Add(item.Subscribe(weaponItemContainerBinders[i].Bind));
                }
            }
            
            for (int i = 0; i < 5; i++)
            {
                var slot = _equipmentWindowViewModel.GetSlotContainer((ItemType.Artefact, i));
                var item = _equipmentWindowViewModel.GetItemContainer((ItemType.Artefact, i));

                if (slot != null && i < artefactSlotContainerBinders.Length)
                {
                    _disposables.Add(slot.Subscribe(artefactSlotContainerBinders[i].Bind));
                }

                if (item != null && i < artefactItemContainerBinders.Length)
                {
                    _disposables.Add(item.Subscribe(artefactItemContainerBinders[i].Bind));
                }
            }

            equipmentWindowViewModel.Init();
		}

        public void RequestItemAddToContainer(ItemViewModel itemViewModel, string containerId)
        {
            var container = _equipmentWindowViewModel.GetItemContainer(containerId);
            if (container == null)
            {
                Debug.LogError($"[EquipmentViewModel] Cannot find container with id: {containerId}");
                return;
            }

            container.TryAddItem(itemViewModel);
        }
        
        public void ApplyNewPlacementInContainer(ItemViewModel itemViewModel, ItemContainerViewModel containerVM, List<InventorySlotViewModel> slotVMs)
        {
            AddItemIfNeeded(itemViewModel, containerVM);
            
            var containerBinder = (ItemContainerBinder)Registry.GetBinder(containerVM);
            var slotContainer = containerVM.SlotContainer;
            var grid = slotContainer.SlotsGrid.CurrentValue;

            Vector2Int newPos = containerBinder.RelevantSlotContainerBinder.TryGetTopLeftSlot(slotVMs, itemViewModel.ItemRows, itemViewModel.ItemColumns);

            if (newPos == new Vector2(-1, -1))
            {
                newPos = slotContainer.SlotsGrid.CurrentValue.GetIndexOf(slotVMs[0]);
            }
            
            LockSlots(grid, newPos, itemViewModel);
            containerBinder.RelevantSlotContainerBinder.PlaceItemInGrid(itemViewModel, newPos.x,
                newPos.y);
        }

        public void ClearPreviousPlacementInContainer(ItemViewModel itemViewModel, ItemContainerViewModel itemContainerViewModel)
        {
            itemContainerViewModel.RemoveItem(itemViewModel);
        }
        private void AddItemIfNeeded(ItemViewModel item, ItemContainerViewModel container)
        {
            if (!container.Items.Contains(item))
                container.TryAddItem(item);
        }
        private void LockSlots(Grid2D<InventorySlotViewModel> grid, Vector2Int start, ItemViewModel item)
        {
            for (int i = 0; i < item.ItemRows; i++)
            {
                for (int j = 0; j < item.ItemColumns; j++)
                {
                    var slot = grid[start.x + i, start.y + j];
                    slot.LockSlot(item);
                }
            }
        }
	}
}