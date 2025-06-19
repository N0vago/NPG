using NPG.Codebase.Game.Gameplay.UI.Root;
using UnityEngine;
using UnityEngine.UI;
using R3;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot
{
    public class InventorySlotBinder : Binder<InventorySlotViewModel>
    {
        [SerializeField] private Image slotImage;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color hoverColor = Color.red;
        
        private RectTransform _slotRectTransform;
        public bool IsAvailable => ViewModel.IsAvailable;
        
        protected override void OnBind(InventorySlotViewModel viewModel)
        {
            base.OnBind(viewModel);
            
            ViewModel.SlotRectTransform = _slotRectTransform;
        }
        
        public void SetSlotColor(SlotColor slotColor)
        {
            if (slotColor == SlotColor.Default)
            {
                slotImage.color = defaultColor;
            }
            else if(slotColor == SlotColor.Hover)
            {
                slotImage.color = hoverColor;
            }
            else
            {
                Debug.LogWarning($"Unknown SlotColor: {slotColor} in: {gameObject.name}");
            }
        }
        private void Awake()
        {
            _slotRectTransform = GetComponent<RectTransform>();
        }
    }

    public enum SlotColor
    {
        Default,
        Hover
    }
}