using System.Collections.Generic;
using System.Linq;
using ModestTree;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot;
using NPG.Codebase.Game.Gameplay.Weapon;
using ObservableCollections;
using R3;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item
{
    public class ItemViewModel : ViewModel
    {
        private readonly ItemSetting _itemSetting;
        
        private readonly Subject<Vector2> _position = new();
        
        private readonly ObservableDictionary<ItemContainerViewModel, List<InventorySlotViewModel>> _slots = new();
        public Observable<Vector2> Position => _position;
        public IReadOnlyObservableDictionary<ItemContainerViewModel, List<InventorySlotViewModel>> Slots => _slots;
        public string ItemID { get; private set; }
        public ItemType ItemType => _itemSetting.itemType;
        public int ItemRows => _itemSetting.itemOccupiedRows;
        public int ItemColumns => _itemSetting.itemOccupiedColumns;
        public WeaponBase WeaponPrefab => _itemSetting.weaponPrefab;

        public ItemViewModel(ItemSetting itemSetting, string itemID)
        {
            ItemID = itemID;
            _itemSetting = itemSetting;
        }
        
        
        public void SetItemHolders(ItemContainerViewModel itemContainer, List<InventorySlotViewModel> slots)
        {
            if ((itemContainer == null || slots == null) && !_slots.IsEmpty()) return;

            _slots.TryAdd(itemContainer, slots);
        }
        
        public void ChangeItemHolders(ItemContainerViewModel itemContainer, List<InventorySlotViewModel> slots)
        {
            var oldEntries = _slots.ToList();
            

            foreach (var entry in oldEntries)
            {
                _slots.Remove(entry.Key);
            }
            
            _slots.Add(itemContainer, slots);
        }
        
        public void ApplyNewPosition(Vector2 newPosition)
        {
            _position.OnNext(newPosition);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_slots != null)
            {
                _slots.Clear();
            }
        }
    }
}