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
        
        protected CompositeDisposable _disposables = new CompositeDisposable();
        protected override void OnBind(WindowViewModel viewModel)
        {
            base.OnBind(viewModel);
            
            viewModel.HideRequested.Subscribe(_ => HideWindow()).AddTo(_disposables);
            
            viewModel.ShowRequested.Subscribe(_ => ShowWindow()).AddTo(_disposables);
        }

        private void HideWindow()
        {
            Debug.Log("Hiding Window: " + ViewModel.Id);
			_canvas.enabled = false;
            OnHide();
		}
        
        private void ShowWindow()
        {
            Debug.Log("Showing Window: " + ViewModel.Id);
			_canvas.enabled = true;
            OnShow();
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

        protected virtual void OnHide()
        {
		}

        protected virtual void OnShow()
        {
		}
		private void Awake() => OnAwake();

        private void OnEnable() => OnEnabling();


        private void OnDisable() => OnDisabling();

        private void OnDestroy() => OnDestroying();

    }
}