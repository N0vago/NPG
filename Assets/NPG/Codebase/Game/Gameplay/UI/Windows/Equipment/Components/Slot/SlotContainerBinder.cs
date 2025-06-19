using NPG.Codebase.Game.Gameplay.UI.Root;
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

        private SlotFactory _slotFactory;

        private CompositeDisposable _disposables = new CompositeDisposable();

        public RectTransform ContainerRectTransform => containerRectTransform;
        public GridLayoutGroup GridLayoutGroup => gridLayoutGroup;
        
        public SlotFactory SlotFactory => _slotFactory;

        [Inject]
        public void Construct(SlotFactory slotFactory)
        {
            _slotFactory = slotFactory;
        }

        protected override void OnBind(SlotContainerViewModel viewModel)
        {
            base.OnBind(viewModel);


            Debug.Log("SlotContainerBinder.OnBind called with ViewModel: " + viewModel);
            _disposables.Add(
                ViewModel.Slots.ObserveAdd().Subscribe(slot => AddSlot(slot.Value))
            );
            
            CreateSlots();
        }
        private void CreateSlots()
        {
            Debug.Log("Creating slots for SlotContainerBinder.");
            foreach (var slot in ViewModel.Slots)
            {
                AddSlot(slot);
            }
        }

        private void AddSlot(InventorySlotViewModel slotValue)
        {
            if (slotValue == null) return;

            Debug.Log("Adding slot: " + slotValue);
            _slotFactory.CreateSlot(slotValue, containerRectTransform, ViewModel.ContainedSlotsID);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();

        }
    }
}