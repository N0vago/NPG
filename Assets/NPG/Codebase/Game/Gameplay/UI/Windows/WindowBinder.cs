using System;
using NPG.Codebase.Game.Gameplay.UI.Root;
using UnityEngine;
using R3;

namespace NPG.Codebase.Game.Gameplay.UI.Windows
{   
    [RequireComponent(typeof(Canvas))]
    public class WindowBinder : Binder<WindowViewModel>
    {
        private Canvas _canvas;
        
        private CompositeDisposable _disposables = new CompositeDisposable();
        protected override void OnBind(WindowViewModel viewModel)
        {
            base.OnBind(viewModel);
            
            viewModel.HideRequested.Subscribe(_ => HideWindow()).AddTo(_disposables);
            
            viewModel.ShowRequested.Subscribe(_ => ShowWindow()).AddTo(_disposables);
        }

        public void HideWindow()
        {
            _canvas.enabled = false;
        }
        
        public void ShowWindow()
        {
            _canvas.enabled = true;
        }
        
        public void Close()
        {
            Destroy(gameObject);
        }

        protected virtual void OnAwake()
        {
            _canvas = GetComponent<Canvas>();
        }
        protected virtual void OnEnabling()
        {
            
        }
        protected virtual void OnDisabling()
        {
            
        }

        protected virtual void OnDestroying()
        {
            _disposables.Dispose();
        }
        private void Awake() => OnAwake();

        private void OnEnable() => OnEnabling();


        private void OnDisable() => OnDisabling();

        private void OnDestroy() => OnDestroying();

    }
}