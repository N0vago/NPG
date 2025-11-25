using System.Collections.Generic;
using System.Linq;
using NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components;
using NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components.Item;
using NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components.Slot;
using NPG.Codebase.Infrastructure.IDs;
using NPG.Codebase.Infrastructure.JsonData;
using NPG.Codebase.Infrastructure.ScriptableObjects;
using NPG.Codebase.Infrastructure.Services.DataSaving;
using R3;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment
{
    public class EquipmentWindowViewModel : WindowViewModel, IDataWriter
    {
        private readonly Dictionary<(ItemType, int), ReactiveProperty<SlotContainerViewModel>> _slotContainers = new();
        private readonly Dictionary<(ItemType, int), ReactiveProperty<ItemContainerViewModel>> _itemContainers = new();
        
        private readonly Dictionary<string, ItemContainerViewModel> _containerById = new();

        private ItemDataBase _itemDataBase;
        private ProgressDataHandler _progressDataHandler;

		public override string Id => WindowIDs.EquipmentWindow;

        public EquipmentWindowViewModel(ItemDataBase itemDataBase, ProgressDataHandler progressDataHandler)
        {
			_itemDataBase = itemDataBase;
			_progressDataHandler = progressDataHandler;
			

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

        public void Init()
        {
			_progressDataHandler.RegisterObserver(this);
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
            _progressDataHandler.SaveProgress(this);
            _progressDataHandler.UnregisterObserver(this);
			base.Dispose();

            foreach (var disposable in _slotContainers.Values)
                disposable?.Dispose();

            foreach (var disposable in _itemContainers.Values)
                disposable?.Dispose();
        }
		public void Load(GameData data)
		{
            Debug.Log("EquipmentWindowBinder: Loading equipment data");
			List<InventoryItemData> itemData = data.userData[data.GetCurrentUserIndex()].playerData.inventoryItemData;

			if (itemData == null || itemData.Count == 0)
				return;

			foreach (var item in itemData)
			{
				ItemContainerViewModel container = GetItemContainer(item.ContainerID);

				foreach (var itemId in item.ItemIDs)
				{
					ItemSetting itemSetting = _itemDataBase.TryGetItemSetting(itemId);
					container.TryAddItem(new ItemViewModel(itemSetting, itemId));
				}
			}
		}

		public void Save(ref GameData data)
		{

			Debug.Log("EquipmentWindowBinder: Saving equipment data");
			int userIndex = data.GetCurrentUserIndex();

			data.userData[userIndex].playerData.inventoryItemData.Clear();

			List<ItemContainerViewModel> containers = GetAllItemContainers();

			foreach (var container in containers)
			{
				if (container.Items.Count == 0)
					continue;
				InventoryItemData inventoryItemData = new InventoryItemData()
				{
					ContainerID = container.ContainerID,
					ItemIDs = container.Items.Select(item => item.ItemID).ToList()
				};

				data.userData[userIndex].playerData.inventoryItemData.Add(inventoryItemData);
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
	}
}