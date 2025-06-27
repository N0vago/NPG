using System.Collections.Generic;
using NPG.Codebase.Game.Gameplay.UI.Root;

namespace NPG.Codebase.Infrastructure.BindingRegistration
{
    public class BinderRegistry : IBinderRegistry
    {
        private readonly Dictionary<ViewModel, object> _vmToBinder = new();
        private readonly Dictionary<object, ViewModel> _binderToVm = new();

        public void Register<TViewModel>(TViewModel viewModel, Binder<TViewModel> binder)
            where TViewModel : ViewModel
        {
            _vmToBinder[viewModel] = binder;
            _binderToVm[binder] = viewModel;
        }

        public void Unregister<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModel
        {
            if (_vmToBinder.Remove(viewModel, out var binder))
            {
                _binderToVm.Remove(binder);
            }
        }

        public Binder<TViewModel> GetBinder<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModel
        {
            return _vmToBinder.TryGetValue(viewModel, out var binder) ? binder as Binder<TViewModel> : null;
        }

        public TViewModel GetViewModel<TViewModel>(Binder<TViewModel> binder)
            where TViewModel : ViewModel
        {
            return _binderToVm.TryGetValue(binder, out var vm) ? vm as TViewModel : null;
        }
    }
}