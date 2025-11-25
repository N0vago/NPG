using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components.Item;
using UnityEngine;
using UnityEngine.UI;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components.Slot
{
    public class InventorySlotBinder : Binder<InventorySlotViewModel>
    {
        [SerializeField] private Image slotImage;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color hoverColor = Color.red;
        [SerializeField] private ItemType containedItemType = ItemType.None;
        public bool IsAvailable => ViewModel.IsAvailable;
        public ItemType ContainedItemType => containedItemType;
        public SlotContainerBinder SlotContainerBinder { get; private set; }
        
        protected override void OnBind(InventorySlotViewModel viewModel)
        {
            base.OnBind(viewModel);
            
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
            SlotContainerBinder = GetComponentInParent<SlotContainerBinder>();
        }
    }

    public enum SlotColor
    {
        Default,
        Hover
    }
}