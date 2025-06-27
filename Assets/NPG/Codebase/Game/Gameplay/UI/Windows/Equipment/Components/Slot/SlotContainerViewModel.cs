using System;
using System.Collections.Generic;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item;
using ObservableCollections;
using R3;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot
{
    public class SlotContainerViewModel : ViewModel
    {
        private readonly ReactiveProperty<Grid2D<InventorySlotViewModel>> _slotsGrid;

        private readonly ObservableList<InventorySlotViewModel> _slots = new();
        public ReadOnlyReactiveProperty<Grid2D<InventorySlotViewModel>> SlotsGrid => _slotsGrid;
        public IReadOnlyObservableList<InventorySlotViewModel> Slots => _slots;
        
        public SlotContainerViewModel(Grid2D<InventorySlotViewModel> slotsGrid, Func<InventorySlotViewModel> defaultSlot)
        {
            _slotsGrid = new ReactiveProperty<Grid2D<InventorySlotViewModel>>(slotsGrid);
            
            _slotsGrid.Value.InitializeGrid(defaultSlot, _slots.Add);
        }
        
        //TODO: No problem if new row added but if new column added it can be placed in wrong position on the view
        public void ExpandGrid(int row, int column)
        {
            _slotsGrid.Value.ExpandGrid(row, column, _slotsGrid.Value[0, 0], true, _slots.Add);
        }
        
        public bool CanItemFit(int itemRows, int itemCols)
        {
            var grid = _slotsGrid.Value;
            int gridRows = grid.Rows;
            int gridCols = grid.Columns;

            for (int row = 0; row <= gridRows - itemRows; row++)
            {
                for (int col = 0; col <= gridCols - itemCols; col++)
                {
                    bool fits = true;

                    for (int i = 0; i < itemRows; i++)
                    {
                        for (int j = 0; j < itemCols; j++)
                        {
                            var slot = grid[row + i, col + j];
                            if (!slot.IsAvailable)
                            {
                                fits = false;
                                break;
                            }
                        }

                        if (!fits) break;
                    }

                    if (fits)
                        return true;
                }
            }

            return false;
        }
        public bool TryFindFreeSpaceForItem(int itemRows, int itemCols,
            out List<InventorySlotViewModel> resultSlots, out int startRow, out int startCol)
        {
            resultSlots = null;
            startRow = -1;
            startCol = -1;

            var grid = _slotsGrid.Value;
            int gridRows = grid.Rows;
            int gridCols = grid.Columns;

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
                            var slot = grid[row + i, col + j];
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
                        resultSlots = candidateSlots;
                        startRow = row;
                        startCol = col;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}