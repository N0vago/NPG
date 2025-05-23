using NPG.Codebase.Game.Gameplay.UI.Root;
using R3;
using UnityEngine;

namespace NPG.Codebase.Playground
{
    public class R3Test : MonoBehaviour
    {
        private void Start()
        {
            Exaple();
        }
        
        
        private void Exaple()
        {
            TestViewModel viewModel = new TestViewModel();
            
            viewModel.RequestClose();
            
            viewModel.CloseRequested.Subscribe(_ => OnCloseRequested(viewModel));
            
            viewModel.RequestClose();
        }
        
        private void OnCloseRequested(TestViewModel viewModel)
        {
            Debug.Log($"Close requested for {viewModel.Id}");
            viewModel.Dispose();
        }
    }
    
    public class TestViewModel : ViewModel
    {
        public Observable<ViewModel> CloseRequested => _closeRequested;

        public string Id => "TestViewModel";

        private readonly Subject<ViewModel> _closeRequested = new();

        public void RequestClose()
        {
            _closeRequested.OnNext(this);
        }

        public override void Dispose()
        {
            base.Dispose();
            _closeRequested?.Dispose();
        }
    }
}