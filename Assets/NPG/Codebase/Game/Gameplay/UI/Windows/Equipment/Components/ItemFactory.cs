using System.Collections.Generic;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item;
using UnityEngine;
using Zenject;
using PrefabProvider = NPG.Codebase.Infrastructure.Services.PrefabProviding.PrefabProvider;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components
{
    public class ItemFactory
    {
        private DiContainer _diContainer;
        
        private readonly Dictionary<ItemViewModel, ItemBinder> _items = new();
        public Dictionary<ItemViewModel, ItemBinder> Items => _items;
        
        public ItemFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        public void CreateItem(ItemViewModel itemViewModel, Transform parent)
        {
            if (_items.ContainsKey(itemViewModel)) return;
            
            var gameObject = PrefabProvider.LoadPrefab(
                itemViewModel.ItemID
            );
            var instance = _diContainer.InstantiatePrefab(gameObject, parent);
            
            var itemBinder = instance.GetComponent<ItemBinder>();
            
            itemBinder.Bind(itemViewModel);
            
            itemBinder.transform.SetParent(parent);
            
            _items[itemViewModel] = itemBinder;
        }

        public void DestroyItem(ItemViewModel itemValue)
        {
            Object.Destroy(_items[itemValue]);
            _items.Remove(itemValue);
        }
    }
}