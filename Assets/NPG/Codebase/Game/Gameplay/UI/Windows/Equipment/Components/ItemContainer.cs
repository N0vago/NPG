using ObservableCollections;
using UnityEngine;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components
{
    public class ItemContainer : MonoBehaviour
    {
        
        private SlotContainer _slotContainer;
        
        private ObservableList<InventoryItem> _items = new();
        public IObservableCollection<InventoryItem> Items => _items;
        
        [Inject]
        public void Construct(SlotContainer slotContainer)
        {
            _slotContainer = slotContainer;
        }
        
        public void AddItem(InventoryItem item)
        {
            if (item == null) return;
            _items.Add(item);
            item.transform.SetParent(transform, false);
        }
        
        public void RemoveItem(InventoryItem item)
        {
            if (item == null) return;
            _items.Remove(item);
            Destroy(item.gameObject);
        }
        
        public void ClearItems()
        {
            foreach (var item in _items)
            {
                Destroy(item.gameObject);
            }
            _items.Clear();
        }
    }
}