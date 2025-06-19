using System.Collections.Generic;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot;
using UnityEngine;
using Zenject;
using PrefabProvider = NPG.Codebase.Infrastructure.Services.PrefabProviding.PrefabProvider;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components
{
    public class SlotFactory
    {
        private DiContainer _container;
        
        private Dictionary<InventorySlotViewModel, InventorySlotBinder> _slots = new();
        
        public Dictionary<InventorySlotViewModel, InventorySlotBinder> Slots => _slots;
        
        public SlotFactory(DiContainer container)
        {
            _container = container;
        }
        public void CreateSlot(InventorySlotViewModel slotViewModel, Transform parent, string addressablesID)
        {
            if (_slots.ContainsKey(slotViewModel)) return;
            
            var gameObject = PrefabProvider.LoadPrefab(
                addressablesID
            );
            
            var instance = _container.InstantiatePrefab(gameObject, parent);
            
            var slotBinder = instance.GetComponent<InventorySlotBinder>();
            
            slotBinder.Bind(slotViewModel);
            slotBinder.transform.SetParent(parent);
            _slots[slotViewModel] = slotBinder;
        }
        
        public void CreateSlots(InventorySlotViewModel slotViewModel, Transform parent, int count)
        {
            for (int i = 0; i < count; i++)
            {
                //CreateSlot(slotViewModel, parent);
            }
        }
    }
}