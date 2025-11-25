using System.Collections.Generic;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components.Slot;
using NPG.Codebase.Infrastructure.Factories;
using ObservableCollections;
using R3;
using UnityEngine;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components.Item
{
    public class ItemContainerBinder : Binder<ItemContainerViewModel>
    {
        [SerializeField] private RectTransform containerRectTransform;
        [SerializeField] private SlotContainerBinder relevantSlotContainerBinder;
        
        private EquipmentWindowBinder _equipmentWindowBinder;
        
        private readonly Dictionary<ItemViewModel, CompositeDisposable> _itemDisposables = new();
        private CompositeDisposable _disposable = new();
        
        private ItemFactory _itemFactory;
        
        public RectTransform ContainerRectTransform => containerRectTransform;
        public SlotContainerBinder RelevantSlotContainerBinder => relevantSlotContainerBinder;
        
        [Inject]
        public void Construct(ItemFactory itemFactory)
        {
            _itemFactory = itemFactory;
        }

        protected override void OnBind(ItemContainerViewModel viewModel)
        {
            base.OnBind(viewModel);
            _disposable.Add(ViewModel.Items.ObserveAdd().Subscribe(item =>
            {
                if (_itemDisposables.TryGetValue(item.Value, out var disposable))
                {
                    disposable.Dispose();
                    _itemDisposables.Remove(item.Value);
                }
                
                var itemDisposable = new CompositeDisposable();
                _itemDisposables[item.Value] = itemDisposable;
                
                item.Value.Slots.ObserveAdd()
                    .Subscribe(entry =>
                    {
                        _equipmentWindowBinder.ApplyNewPlacementInContainer(item.Value, entry.Value.Key, entry.Value.Value);
                    })
                    .AddTo(itemDisposable);

                item.Value.Slots.ObserveRemove()
                    .Subscribe(entry =>
                    {
                        _equipmentWindowBinder.ClearPreviousPlacementInContainer(item.Value, entry.Value.Key);
                    })
                    .AddTo(itemDisposable);
                
                if (Registry.GetBinder(item.Value) == null)
                    InitItem(item.Value);
            }));

            _disposable.Add(ViewModel.Items.ObserveRemove().Subscribe(item =>
            {
                RemoveItem(item.Value);
            }));
            
        }

        private void Awake()
        {
            _equipmentWindowBinder = GetComponentInParent<EquipmentWindowBinder>();
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
            foreach (var itemDisposable in _itemDisposables.Values)
            {
                itemDisposable.Dispose();
            }
            _itemDisposables.Clear();
            
        }


        private void RemoveItem(ItemViewModel item)
        {
            foreach (var slot in ViewModel.SlotContainer.SlotsGrid.CurrentValue)
            {
                if(slot.Item.CurrentValue == item)
                {
                    slot.UnlockSlot(item);
                }
            }
        }

        private void InitItem(ItemViewModel item)
        {
            _itemFactory.CreateItem(item, containerRectTransform);

            int itemRows = item.ItemRows;
            int itemCols = item.ItemColumns;

            if (ViewModel.SlotContainer.TryFindFreeSpaceForItem(itemRows, itemCols, out var slots, out var row, out var col))
            {
                foreach (var slot in slots)
                {
                    Debug.Log($"Locking slot {slot.SlotID} for item {item.ItemID}");
                    slot.LockSlot(item);
                }

                item.SetItemHolders(ViewModel, slots);
                relevantSlotContainerBinder.PlaceItemInGrid(item, row, col);
            }
        }
        
    }
}