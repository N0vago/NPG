using System.Collections.Generic;
using ModestTree;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot;
using NPG.Codebase.Infrastructure.Extensions;
using ObservableCollections;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item
{
    public class ItemContainerBinder : Binder<ItemContainerViewModel>
    {
        [SerializeField] private SlotContainerBinder relevantSlotContainerBinder;
        [SerializeField] private RectTransform containerRectTransform;
        
        private CompositeDisposable _disposable = new();
        
        private ItemFactory _itemFactory;
        
        [Inject]
        public void Construct(ItemFactory itemFactory)
        {
            _itemFactory = itemFactory;
            
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }

        protected override void OnBind(ItemContainerViewModel viewModel)
        {
            base.OnBind(viewModel);
            if (ViewModel == null)
            {
                Debug.LogError("ViewModel is null in ItemContainerBinder.OnBind.");
                return;
            }

            if (ViewModel.Items == null)
            {
                Debug.LogError("ViewModel.Items is null in ItemContainerBinder.OnBind.");
                return;
            }
            _disposable.Add(ViewModel.Items.ObserveAdd().Subscribe(item => AddItem(item.Value)));
            
            _disposable.Add(ViewModel.Items.ObserveRemove().Subscribe(item => RemoveItem(item.Value)));
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

        private void AddItem(ItemViewModel item)
        {
            _itemFactory.CreateItem(item, containerRectTransform);
            
            _disposable.Add(item.SelectedSlots.Subscribe(slots => ReplaceItem(item, slots)));
            
            ItemBinder itemBinder = _itemFactory.Items[item];
            
            int itemRows = itemBinder.ItemRow;
            int itemCols = itemBinder.ItemColumn;

            var slotsGrid = ViewModel.SlotContainer.SlotsGrid.CurrentValue;
            int gridRows = slotsGrid.Rows;
            int gridCols = slotsGrid.Columns;
            
            for (int row = 0; row <= gridRows - itemRows; row++)
            {
                for (int col = 0; col <= gridCols - itemCols; col++)
                {
                    bool fits = true;
                    List<InventorySlotViewModel> candidateSlots = new();

                    for (int i = 0; i < itemRows; i++)
                    {
                        for (int j = 0; j < itemCols; j++)
                        {
                            var slot = slotsGrid[row + i, col + j];
                            if (!slot.IsAvailable)
                            {
                                fits = false;
                                break;
                            }

                            candidateSlots.Add(slot);
                        }

                        if (!fits) break;
                    }

                    if (fits)
                    {
                        foreach (var slot in candidateSlots)
                        {
                            slot.LockSlot(item);
                            Debug.Log($"Slot anchor position x: {slot.SlotRectTransform.anchoredPosition.x}, y: {slot.SlotRectTransform.anchoredPosition.y}");
                            
                        }
                        
                        PlaceItemInGrid(itemBinder, row, col, relevantSlotContainerBinder.GridLayoutGroup);
                        return;
                    }
                }
            }

            Debug.LogWarning("Нет подходящих слотов для предмета " + itemBinder.gameObject.name);

        }

        private void ReplaceItem(ItemViewModel itemViewModel, List<InventorySlotBinder> slotBinders)
        {
            ItemBinder itemBinder = _itemFactory.Items[itemViewModel];
            
            List<InventorySlotViewModel> occupiedSlots = ViewModel.SlotContainer.GetOccupiedSlotsForItem(itemViewModel);
            
            foreach (var slot in occupiedSlots)
                slot.UnlockSlot(itemViewModel);

            // Если не выбрано слотов — возвращаем предмет на старое место
            if (slotBinders == null || slotBinders.Count == 0)
            {
                Vector2Int itemIndex = ViewModel.SlotContainer.SlotsGrid.CurrentValue.GetIndexOf(occupiedSlots[0]);
                PlaceItemInGrid(itemBinder, itemIndex.x, itemIndex.y, relevantSlotContainerBinder.GridLayoutGroup);
                foreach (var slot in occupiedSlots)
                    slot.LockSlot(itemViewModel);
                return;
            }

            // Получаем SlotViewModel'ы
            List<InventorySlotViewModel> selectedSlots = new();
            foreach (var slotBinder in slotBinders)
            {
                if (relevantSlotContainerBinder.SlotFactory.Slots.TryGetKeyByValue(slotBinder, out var slotViewModel))
                    selectedSlots.Add(slotViewModel);
            }

            // Проверка валидности слотов
            int itemRows = itemBinder.ItemRow;
            int itemCols = itemBinder.ItemColumn;

            var grid = ViewModel.SlotContainer.SlotsGrid.CurrentValue;

            Vector2Int? topLeft = TryGetTopLeftSlot(selectedSlots, grid, itemRows, itemCols);
            if (topLeft == null)
            {
                Vector2Int fallback = ViewModel.SlotContainer.SlotsGrid.CurrentValue.GetIndexOf(occupiedSlots[0]);
                PlaceItemInGrid(itemBinder, fallback.x, fallback.y, relevantSlotContainerBinder.GridLayoutGroup);
                foreach (var slot in occupiedSlots)
                    slot.LockSlot(itemViewModel);
                return;
            }
            
            for (int i = 0; i < itemRows; i++)
            {
                for (int j = 0; j < itemCols; j++)
                {
                    var slot = grid[topLeft.Value.x + i, topLeft.Value.y + j];
                    slot.LockSlot(itemViewModel);
                }
            }
            
            PlaceItemInGrid(itemBinder, topLeft.Value.x, topLeft.Value.y, relevantSlotContainerBinder.GridLayoutGroup);
                    
        }

        private void PlaceItemInGrid(ItemBinder itemBinder, int startRow, int startCol, GridLayoutGroup gridLayoutGroup)
        {
            int itemRows = itemBinder.ItemRow;
            int itemCols = itemBinder.ItemColumn;

            Vector2 position = CalculateItemCenterPosition(startRow, startCol,itemRows, itemCols, gridLayoutGroup);
            
            itemBinder.AttachItemToPosition(position);
        }

        private Vector2 CalculateItemCenterPosition(int startRow, int startCol, int itemRows, int itemCols, GridLayoutGroup grid)
        {
            var cellSize = grid.cellSize;
            var spacing = grid.spacing;
            var padding = grid.padding;

            
            float leftX = padding.left + startCol * (cellSize.x + spacing.x);
            float topY = padding.top + startRow * (cellSize.y + spacing.y);

            
            float rightX = leftX + itemCols * cellSize.x + (itemCols - 1) * spacing.x;
            float bottomY = topY + itemRows * cellSize.y + (itemRows - 1) * spacing.y;

            
            float centerX = (leftX + rightX) / 2f;
            float centerY = (topY + bottomY) / 2f;

            return new Vector2(centerX, -centerY);
        }
        
        private Vector2Int? TryGetTopLeftSlot(List<InventorySlotViewModel> selectedSlots, Grid2D<InventorySlotViewModel> grid, int itemRows, int itemCols)
        {
            for (int row = 0; row <= grid.Rows - itemRows; row++)
            {
                for (int col = 0; col <= grid.Columns - itemCols; col++)
                {
                    bool fits = true;

                    for (int i = 0; i < itemRows; i++)
                    {
                        for (int j = 0; j < itemCols; j++)
                        {
                            var slot = grid[row + i, col + j];
                            if (!selectedSlots.Contains(slot) || !slot.IsAvailable)
                            {
                                fits = false;
                                break;
                            }
                        }
                        if (!fits) break;
                    }

                    if (fits)
                        return new Vector2Int(row, col);
                }
            }

            return null;
        }


        
    }
}