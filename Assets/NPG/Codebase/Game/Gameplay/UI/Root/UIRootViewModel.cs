using System;
using System.Collections.Generic;
using NPG.Codebase.Game.Gameplay.UI.Windows;
using ObservableCollections;
using R3;

namespace NPG.Codebase.Game.Gameplay.UI.Root
{
    public class UIRootViewModel : IDisposable
    {
        public ReadOnlyReactiveProperty<ViewModel> OpenedScreen => _openedScreen;
        
        public IObservableCollection<WindowViewModel> OpenedWindows => _openedWindows;
        
        private readonly ReactiveProperty<ViewModel> _openedScreen = new();
        private readonly ObservableList<WindowViewModel> _openedWindows = new();
        private readonly Dictionary<ViewModel, IDisposable> _viewModelSubscriptions = new();

        public void Dispose()
        {
            CloseAllWindows();
            _openedScreen?.Dispose();
        }
        
        public void OpenScreen(ViewModel screenViewModel)
        {
            _openedScreen.Value?.Dispose();
            _openedScreen.Value = screenViewModel;
        }

        public void OpenWindow(WindowViewModel window)
        {
            if (_openedWindows.Contains(window))
            {
                return;
            }

            var subscription = window.CloseRequested.Subscribe(_ => CloseWindow(window));
            _viewModelSubscriptions.Add(window, subscription);
            _openedWindows.Add(window);
        }

        public void CloseWindow(WindowViewModel window)
        {
            if (_openedWindows.Contains(window))
            {
                window.Dispose();
                _openedWindows.Remove(window);

                var viewModelSubscription = _viewModelSubscriptions[window];
                viewModelSubscription?.Dispose();
                _viewModelSubscriptions.Remove(window);
            }
        }
        public void CloseAllWindows()
        {
            foreach (var window in _openedWindows)
            {
                CloseWindow(window);
            }
        }
    }
}