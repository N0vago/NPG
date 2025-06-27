using NPG.Codebase.Game.Gameplay.UI.Root;
using R3;

namespace NPG.Codebase.Game.Gameplay.UI.Windows
{
    public abstract class WindowViewModel : ViewModel
    {
        public Observable<ViewModel> CloseRequested => _closeRequested;
        
        public Observable<ViewModel> HideRequested => _hideRequested;
        
        public Observable<ViewModel> ShowRequested => _showRequested;

        private readonly Subject<ViewModel> _closeRequested = new();
        
        private readonly Subject<ViewModel> _hideRequested = new();
        
        private readonly Subject<ViewModel> _showRequested = new();
        
        public bool IsVisible { get; protected set; } = true;
        
        public abstract string Id { get; }

        public void RequestHide()
        {
            if (!IsVisible)
            {
                return;
            }
            IsVisible = false;
            _hideRequested.OnNext(this);
        }
        
        public void RequestShow()
        {
            if (IsVisible)
            {
                return;
            }
            IsVisible = true;
            _showRequested.OnNext(this);
        }
        
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