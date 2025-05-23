using NPG.Codebase.Game.Gameplay.UI.Root;
using R3;

namespace NPG.Codebase.Game.Gameplay.UI.Windows
{
    public abstract class WindowViewModel : ViewModel
    {
        public Observable<ViewModel> CloseRequested => _closeRequested;
        
        public abstract string Id { get; }

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