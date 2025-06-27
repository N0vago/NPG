using NPG.Codebase.Game.Gameplay.UI.Root;

namespace NPG.Codebase.Infrastructure.BindingRegistration
{
    public interface IBinderRegistry
    {
        void Register<TViewModel>(TViewModel viewModel, Binder<TViewModel> binder)
            where TViewModel : ViewModel;

        void Unregister<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModel;

        Binder<TViewModel> GetBinder<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModel;

        TViewModel GetViewModel<TViewModel>(Binder<TViewModel> binder)
            where TViewModel : ViewModel;
    }
}