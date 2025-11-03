using System.Collections.Generic;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item;
using NPG.Codebase.Infrastructure.Factories;
using ObservableCollections;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot
{
    public class SlotContainerBinder : Binder<SlotContainerViewModel>
    {
        [SerializeField] private RectTransform containerRectTransform;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private ItemContainerBinder relevantItemContainerBinder;

        private SlotFactory _slotFactory;

        private CompositeDisposable _disposables = new CompositeDisposable();
        
        public ItemContainerBinder RelevantItemContainerBinder => relevantItemContainerBinder;

        [Inject]
        public void Construct(SlotFactory slotFactory)
        {
            _slotFactory = slotFactory;
        }

        public void PlaceItemInGrid(ItemViewModel itemViewModel, int startRow, int startCol)
        {
            int itemRows = itemViewModel.ItemRows;
            int itemCols = itemViewModel.ItemColumns;

            Debug.LogWarning("Placing item in grid at row: " + startRow + ", col: " + startCol +
                             ", rows: " + itemRows + ", cols: " + itemCols);
            Vector2 position = CalculateItemCenterPosition(startRow, startCol,itemRows, itemCols);
            
            itemViewModel.ApplyNewPosition(position);
        }

        public Vector2 CalculateItemCenterPosition(int startRow, int startCol, int itemRows, int itemCols)
        {
            var cellSize = gridLayoutGroup.cellSize;
            var spacing = gridLayoutGroup.spacing;
            var padding = gridLayoutGroup.padding;

            
            float leftX = padding.left + startCol * (cellSize.x + spacing.x);
            float topY = padding.top + startRow * (cellSize.y + spacing.y);

            
            float rightX = leftX + itemCols * cellSize.x + (itemCols - 1) * spacing.x;
            float bottomY = topY + itemRows * cellSize.y + (itemRows - 1) * spacing.y;

            
            float centerX = (leftX + rightX) / 2f;
            float centerY = (topY + bottomY) / 2f;

            return new Vector2(centerX, -centerY);
        }
        
        public Vector2Int TryGetTopLeftSlot(List<InventorySlotViewModel> selectedSlots, int itemRows, int itemCols)
        {
            for (int row = 0; row <= ViewModel.SlotsGrid.CurrentValue.Rows - itemRows; row++)
            {
                for (int col = 0; col <= ViewModel.SlotsGrid.CurrentValue.Columns - itemCols; col++)
                {
                    bool fits = true;

                    for (int i = 0; i < itemRows; i++)
                    {
                        for (int j = 0; j < itemCols; j++)
                        {
                            var slot = ViewModel.SlotsGrid.CurrentValue[row + i, col + j];
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

            return new Vector2Int(-1, -1);
        }

        protected override void OnBind(SlotContainerViewModel viewModel)
        {
            base.OnBind(viewModel);
            
            _disposables.Add(ViewModel.Slots.ObserveAdd().Subscribe(slot => AddSlot(slot.Value)));
            
            CreateSlots();
        }

        private void CreateSlots()
        {
            foreach (var slot in ViewModel.Slots)
            {
                AddSlot(slot);
            }
        }

        private void AddSlot(InventorySlotViewModel slotValue)
        {
            if (slotValue == null) return;
            
            _slotFactory.CreateSlot(slotValue, containerRectTransform);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();

        }
    }
}