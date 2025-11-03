using System;
using System.Collections.Generic;
using System.Linq;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot;
using ObservableCollections;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item
{
    public class ItemBinder : Binder<ItemViewModel>, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform itemRectTransform;
        [SerializeField] private Image itemIcon;
        
        private GraphicRaycaster _raycaster;
        
        private Vector2[] _raycastOffsets;
        
        private List<InventorySlotBinder> _hoveredSlots = new();
        private List<InventorySlotBinder> _selectedSlots = new();
        
        private ItemContainerBinder _newContainerBinder;
        private ItemContainerBinder _currentContainerBinder;
        
        private CompositeDisposable _disposables = new CompositeDisposable();
        
        protected override void OnBind(ItemViewModel viewModel)
        {
            base.OnBind(viewModel);

            _disposables.Add(ViewModel.Position.Subscribe(newPos 
                => itemRectTransform.anchoredPosition = newPos
            ));

            _disposables.Add(ViewModel.Slots.ObserveAdd().Subscribe(entry =>
            {
                _currentContainerBinder = (ItemContainerBinder)Registry.GetBinder(entry.Value.Key);
                _selectedSlots = entry.Value.Value
                    .Select(slotViewModel => (InventorySlotBinder)Registry.GetBinder(slotViewModel))
                    .ToList();
            }));
            
            AutoFillRaycastOffsets();
        }
        

        public void OnBeginDrag(PointerEventData eventData)
        {
            itemIcon.raycastTarget = false;
            
            transform.SetParent(transform.root, true);
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;

            _hoveredSlots.ForEach(slot => slot.SetSlotColor(SlotColor.Default));
            _hoveredSlots.Clear();

            foreach (Vector2 offset in _raycastOffsets)
            {
                Vector2 worldPoint = (Vector2)itemRectTransform.position + offset;
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, worldPoint);

                var raycastEvent = new PointerEventData(EventSystem.current)
                {
                    position = screenPoint
                };

                List<RaycastResult> results = new();
                _raycaster.Raycast(raycastEvent, results);

                foreach (var result in results)
                {
                    if (!result.gameObject.TryGetComponent<InventorySlotBinder>(out var slotBinder)) continue;
                    if (!slotBinder.IsAvailable || (slotBinder.ContainedItemType & ViewModel.ItemType) == 0) continue;

                    var containerBinder = slotBinder.SlotContainerBinder?.RelevantItemContainerBinder;
                    
                    _newContainerBinder =  containerBinder ?? _currentContainerBinder;

                    if (!_hoveredSlots.Contains(slotBinder))
                    {
                        _hoveredSlots.Add(slotBinder);
                        slotBinder.SetSlotColor(SlotColor.Hover);
                    }
                }
            }
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            itemIcon.raycastTarget = true;
            
            if (_hoveredSlots.Count == ViewModel.ItemRows * ViewModel.ItemColumns)
            {
                _hoveredSlots.ForEach(slot => slot.SetSlotColor(SlotColor.Default));
                ViewModel.ChangeItemHolders(Registry.GetViewModel(_newContainerBinder), _hoveredSlots.ConvertAll(slot => Registry.GetViewModel(slot)));
                transform.SetParent(_newContainerBinder.ContainerRectTransform, false);
                transform.SetAsLastSibling();
            }
            else
            {
                _hoveredSlots.ForEach(slot => slot.SetSlotColor(SlotColor.Default));
                ViewModel.ChangeItemHolders(Registry.GetViewModel(_currentContainerBinder), _selectedSlots.ConvertAll(slot => Registry.GetViewModel(slot)));
                transform.SetParent(_currentContainerBinder.ContainerRectTransform, false);
                transform.SetAsLastSibling();
                Debug.LogWarning("No hovered slots found for item drop.");
            }
        }

        private void AutoFillRaycastOffsets()
        {
            const float cellSize = 50f;

            int cols  = ViewModel.ItemColumns;
            int rows = ViewModel.ItemRows;

            float totalWidth = cols * cellSize;
            float totalHeight = rows * cellSize;

            Vector2[] offsets = new Vector2[cols * rows];
            int index = 0;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    float offsetX = (x + 0.5f) * cellSize;
                    float offsetY = (y + 0.5f) * cellSize;
                    
                    offsetX -= totalWidth / 2f;
                    offsetY -= totalHeight / 2f;

                    offsets[index++] = new Vector2(offsetX, offsetY);
                }
            }

            _raycastOffsets = offsets;
        }

        private void Awake()
        {
            _raycaster = GetComponentInParent<GraphicRaycaster>();
            
        }
        private void OnDestroy()
        {
            _hoveredSlots.Clear();
            _disposables.Dispose();
        }
    }
}