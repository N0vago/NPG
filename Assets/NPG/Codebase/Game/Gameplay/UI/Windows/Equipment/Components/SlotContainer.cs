using System.Collections.Generic;
using ObservableCollections;
using UnityEngine;
using R3;
using UnityEngine.UI;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class SlotContainer : MonoBehaviour
    {
        [SerializeField] private Matrix2D<InventorySlot> slots;
        private GridLayoutGroup _gridLayoutGroup;

        private ItemContainer _itemContainer;
        
        private CompositeDisposable _disposables = new CompositeDisposable();
        

        [Inject]
        public void Construct(ItemContainer itemContainer)
        {
            _itemContainer = itemContainer;
        }
        public List<InventorySlot> GetAvailableSlotsForItem(InventoryItem item)
        {
            List<InventorySlot> availableSlots = new List<InventorySlot>();

            int targetRows = item.SlotsToOccupy.rows;
            int targetCols = item.SlotsToOccupy.cols;
            
            for (int r = 0; r <= slots.rows - targetRows; r++)
            {
                for (int c = 0; c <= slots.cols - targetCols; c++)
                {
                    bool canPlace = true;
                    for (int tr = 0; tr < targetRows; tr++)
                    {
                        for (int tc = 0; tc < targetCols; tc++)
                        {
                            if (slots[r + tr, c + tc].IsOccupied)
                            {
                                canPlace = false;
                                break;
                            }
                        }
                        if (!canPlace) break;
                    }

                    if (canPlace)
                    {
                        for (int tr = 0; tr < targetRows; tr++)
                        {
                            for (int tc = 0; tc < targetCols; tc++)
                            {
                                availableSlots.Add(slots[r + tr, c + tc]);
                            }
                        }

                        return availableSlots;
                    }
                }
            }

            return null;
        }

        public List<InventorySlot> GetSlotsToClear(InventoryItem item)
        {
            int targetRows = item.SlotsToOccupy.rows;
            int targetCols = item.SlotsToOccupy.cols;

            List<InventorySlot> slotsToClear = new();
            for (int r = 0; r < slots.rows; r++)
            {
                for (int c = 0; c < slots.cols; c++)
                {
                    if (slots[r, c].CurrentItem.CurrentValue == item)
                    {
                        for (int tr = 0; tr < targetRows; tr++)
                        {
                            for (int tc = 0; tc < targetCols; tc++)
                            {
                                slotsToClear.Add(slots[r + tr, c + tc]);
                            }
                        }

                        return slotsToClear;
                    }
                }
            }

            return null;
        }

        private void Awake()
        {
            _gridLayoutGroup = GetComponent<GridLayoutGroup>();
            for (int r = 0; r < slots.rows; r++)
            {
                for (int c = 0; c < slots.cols; c++)
                {
                    InventorySlot slot = slots[r, c];
                    if (slot != null)
                    {
                        _disposables.Add(slot.CurrentItem.Subscribe(item => OnItemChanged(item, slot)));
                    }
                }
            }
        }

        private void Start()
        {
            _disposables.Add(_itemContainer.Items.ObserveAdd().Subscribe(e => OnItemAdd(e.Value)));
            _disposables.Add(_itemContainer.Items.ObserveRemove().Subscribe(e => OnItemRemove(e.Value)));
        }
        
        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        private void OnItemRemove(InventoryItem item)
        {
            List<InventorySlot> slotsToClear = GetSlotsToClear(item);
            
            if (slotsToClear != null)
            {
                foreach (var slot in slotsToClear)
                {
                    slot.ClearOccupied();
                }
            }
        }

        private void OnItemAdd(InventoryItem item)
        {
            List<InventorySlot> availableSlots = GetAvailableSlotsForItem(item);
            foreach (var availableSlot in availableSlots)
            {
                availableSlot.SetOccupied(true, item);
            }
        }



        private void OnItemChanged(InventoryItem item, InventorySlot slot)
        {

        }
    }
}