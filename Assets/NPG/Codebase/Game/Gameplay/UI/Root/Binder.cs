using NPG.Codebase.Infrastructure.BindingRegistration;
using UnityEngine;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Root
{
    public abstract class Binder<T> : MonoBehaviour where T : ViewModel
    {
        protected T ViewModel;
        
        protected IBinderRegistry Registry;
        
        [Inject]
        protected virtual void Construct(IBinderRegistry registry)
        {
            Registry = registry;
        }
        public void Bind(T viewModel)
        {
            ViewModel = viewModel;
            Registry.Register(viewModel, this);
            OnBind(viewModel);
        }
        
        protected virtual void OnBind(T viewModel){}
    }
}