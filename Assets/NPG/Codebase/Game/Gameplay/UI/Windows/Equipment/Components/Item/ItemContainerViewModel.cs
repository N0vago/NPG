using System;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot;
using ObservableCollections;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item
{
    public class ItemContainerViewModel : ViewModel, IDisposable
    {
        private readonly SlotContainerViewModel _slotContainer;
        
        private ObservableList<ItemViewModel> _items = new ObservableList<ItemViewModel>();
        public IObservableCollection<ItemViewModel> Items => _items;
        public SlotContainerViewModel SlotContainer => _slotContainer;
        public ItemContainerViewModel(SlotContainerViewModel slotContainer)
        {
            _slotContainer = slotContainer;
        }

        public void AddItem(ItemViewModel itemViewModel) => _items.Add(itemViewModel);

        public void RemoveItem(ItemViewModel itemViewModel) => _items.Remove(itemViewModel);

        public override void Dispose()
        {
            base.Dispose(); ;
            _items.Clear();
        }
        
        
    }
}