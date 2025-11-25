using NPG.Codebase.Game.Gameplay.UI.Factories;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Infrastructure.JsonData;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Menu
{
	public class MenuBinder : Binder<MenuViewModel>
	{
		[SerializeField] private Button _newGameButton;
		[SerializeField] private Button _continueGameButton;
		[SerializeField] private Button _exitButton;

		[SerializeField] private Button _changeProfileButton;

		private UserProfileData _currentUser;

		WindowsFactory _windowsFactory;

		private SceneContext _container;

		private CompositeDisposable _disposables = new CompositeDisposable();

		private void Awake()
		{
			_container = UnityEngine.Object.FindObjectOfType<SceneContext>();
			_windowsFactory = _container.Container.Resolve<WindowsFactory>();
		}
		protected override void OnBind(MenuViewModel viewModel)
		{
			_disposables.Add(viewModel.CurrentUser.Subscribe(user =>
			{
				_currentUser = user;
			}));
		}

		private void OnEnable()
		{
			_newGameButton.onClick.AddListener(OnNewGameClicked);
			_continueGameButton.onClick.AddListener(OnContinueGameClicked);
			_exitButton.onClick.AddListener(OnExitClicked);
			_changeProfileButton.onClick.AddListener(OnChangeProfileClicked);
		}
		private void OnDisable()
		{
			_newGameButton.onClick.RemoveListener(OnNewGameClicked);
			_continueGameButton.onClick.RemoveListener(OnContinueGameClicked);
			_exitButton.onClick.RemoveListener(OnExitClicked);
			_changeProfileButton.onClick.RemoveListener(OnChangeProfileClicked);
		}

		private void OnChangeProfileClicked()
		{
			
		}

		private void OnExitClicked()
		{
			Debug.Log("Exit Clicked");
		}

		private void OnContinueGameClicked()
		{
			Debug.Log("Continue Game Clicked");
		}

		private void OnNewGameClicked()
		{
			Debug.Log("New Game Clicked");
		}
	}
}
