using System.Collections.Generic;
using NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components.Slot;
using UnityEngine;
using Zenject;
using PrefabProvider = NPG.Codebase.Infrastructure.Services.PrefabProviding.PrefabProvider;

namespace NPG.Codebase.Infrastructure.Factories
{
    public class SlotFactory
    {
        private DiContainer _container;
        
        private Dictionary<InventorySlotViewModel, InventorySlotBinder> _slots = new();
        
        public SlotFactory(DiContainer container)
        {
            _container = container;
        }
        public void CreateSlot(InventorySlotViewModel slotViewModel, Transform parent)
        {
            if (_slots.ContainsKey(slotViewModel)) return;
            
            var gameObject = PrefabProvider.LoadPrefab(
                slotViewModel.SlotID
            );
            
            var instance = _container.InstantiatePrefab(gameObject, parent);
            
            var slotBinder = instance.GetComponent<InventorySlotBinder>();
            
            slotBinder.Bind(slotViewModel);
            slotBinder.transform.SetParent(parent);
            _slots[slotViewModel] = slotBinder;
        }
    }
}