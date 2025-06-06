using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class InventoryItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Matrix2D slotsToOccupy;
        [SerializeField] private Vector2 slotSize;
        [SerializeField] private Image itemImage;
        
        private SlotContainer _slotContainer;
        private Vector2 LockedSize => new(slotsToOccupy.cols * slotSize.x, slotsToOccupy.rows * slotSize.y);
        
        private RectTransform _rectTransform;
        public Matrix2D SlotsToOccupy => slotsToOccupy;
        
        [Inject]
        public void Construct(SlotContainer slotContainer)
        {
            _slotContainer = slotContainer;
        }
        
        void OnEnable()
        {
            _rectTransform = GetComponent<RectTransform>();
            ApplyLock();
        }

        
#if UNITY_EDITOR
        void Update()
        {
            if (!Application.isPlaying)
            {
                if (_rectTransform != null && _rectTransform.sizeDelta != LockedSize)
                {
                    _rectTransform.sizeDelta = LockedSize;
                }
            }
        }
#endif

        private void ApplyLock()
        {
            if (_rectTransform != null)
                _rectTransform.sizeDelta = LockedSize;
        }
        public void OnDrag(PointerEventData eventData)
        {
            itemImage.raycastTarget = false; // Disable raycast target to allow dragging through UI
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        
    }
}