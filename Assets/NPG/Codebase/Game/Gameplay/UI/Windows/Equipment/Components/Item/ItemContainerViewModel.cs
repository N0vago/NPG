using System;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot;
using ObservableCollections;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item
{
    public class ItemContainerViewModel : ViewModel
    {
        private readonly SlotContainerViewModel _slotContainer;
        
        private ObservableList<ItemViewModel> _items = new();
        public IObservableCollection<ItemViewModel> Items => _items;
        public SlotContainerViewModel SlotContainer => _slotContainer;
        
        public string ContainerID { get; private set; }
        public ItemContainerViewModel(SlotContainerViewModel slotContainer, string containerID)
        {
            ContainerID = containerID;
            _slotContainer = slotContainer ?? throw new ArgumentNullException(nameof(slotContainer));
        }

        public bool TryAddItem(ItemViewModel item)
        {
           
            if (_slotContainer.CanItemFit(item.ItemRows, item.ItemColumns))
            { 
                _items.Add(item);
                return true;
            }
            
            Debug.LogWarning("Item cannot fit in the container: " + ContainerID);
            return false;
        }

        public void RemoveItem(ItemViewModel itemViewModel) => _items.Remove(itemViewModel);
        

        public override void Dispose()
        {
            base.Dispose(); ;
            _items.Clear();
        }
    }
}