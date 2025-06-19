using System;
using System.Collections.Generic;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item
{
    public class ItemBinder : Binder<ItemViewModel>, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private ItemSettings itemSettings;
        [SerializeField] private RectTransform itemRectTransform;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Vector2[] raycastOffsets;

        private GraphicRaycaster _raycaster;
        private List<InventorySlotBinder> _hoveredSlots = new List<InventorySlotBinder>();
        
        private CompositeDisposable _disposable = new CompositeDisposable();
        
        public int ItemRow => itemSettings.itemOccupiedRows;
        public int ItemColumn => itemSettings.itemOccupiedColumns;
        
        protected override void OnBind(ItemViewModel viewModel)
        {
            base.OnBind(viewModel);

            _raycaster = GetComponentInParent<GraphicRaycaster>();
        }
        
        public void AttachItemToPosition(Vector2 position)
        {
            itemRectTransform.anchoredPosition = position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            itemIcon.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;

            _hoveredSlots.ForEach(slot => slot.SetSlotColor(SlotColor.Default));
            
            _hoveredSlots.Clear();

            foreach (Vector2 offset in raycastOffsets)
            {
                Vector2 worldPoint = (Vector2)itemRectTransform.position + offset;

                var raycastEvent = new PointerEventData(EventSystem.current)
                {
                    position = worldPoint
                };

                List<RaycastResult> results = new();
                _raycaster.Raycast(raycastEvent, results);

                foreach (var result in results)
                {
                    if (result.gameObject.TryGetComponent<InventorySlotBinder>(out var slotBinder))
                    {
                        if (slotBinder.IsAvailable)
                        {
                            slotBinder.SetSlotColor(SlotColor.Hover);
                            _hoveredSlots.Add(slotBinder);
                        }
                    }
                }
            }
        }
        

        public void OnEndDrag(PointerEventData eventData)
        {
            itemIcon.raycastTarget = true;
            
            if (_hoveredSlots.Count > 0)
            {
                ViewModel.AddSelectedSlot(_hoveredSlots);
                _hoveredSlots.ForEach(slot => slot.SetSlotColor(SlotColor.Default));
            }
            else
            {
                ViewModel.AddSelectedSlot(null);
                Debug.LogWarning("No hovered slots found for item drop.");
            }
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
            _hoveredSlots.Clear();
        }
    }
}