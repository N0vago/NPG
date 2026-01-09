using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Infrastructure.JsonData;
using NPG.Codebase.Infrastructure.ScriptableObjects;
using NPG.Codebase.Infrastructure.Services.DataSaving;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using PrefabProvider = NPG.Codebase.Infrastructure.Services.PrefabProviding.PrefabProvider;
namespace NPG.Codebase.Game.Gameplay.UI.Menu
{
	public class MenuBinder : Binder<MenuViewModel>
	{
		[SerializeField] private Button newGameButton;
		[SerializeField] private Button continueGameButton;
		[SerializeField] private Button exitButton;

		[SerializeField] private Button changeProfileButton;
		
		[SerializeField] private TMP_Text currentUserName;
		[SerializeField] private Image currentUserAvatar;
		
		private MenuViewModel _viewModel;
		
		private UIRootViewModel _uiRootViewModel;
		private ProgressDataHandler _progressDataHandler;
		private CompositeDisposable _disposables = new CompositeDisposable();
		
		[Inject]
		public void Construct(UIRootViewModel uiRootViewModel, ProgressDataHandler progressDataHandler)
		{
			_uiRootViewModel = uiRootViewModel;
			_progressDataHandler = progressDataHandler;
		}
		protected override void OnBind(MenuViewModel viewModel)
		{
			
			_viewModel = viewModel;
			_disposables.Add(viewModel.CurrentUser.Subscribe(user =>
			{
				currentUserName.text = user.userName;
				currentUserAvatar.sprite = PrefabProvider.LoadAsset<Sprite>(user.userAvatar);
			}));

			continueGameButton.interactable = _viewModel.CurrentUser.CurrentValue.hasSession;
		}

		private void OnEnable()
		{
			newGameButton.onClick.AddListener(OnNewGameClicked);
			continueGameButton.onClick.AddListener(OnContinueGameClicked);
			exitButton.onClick.AddListener(OnExitClicked);
			changeProfileButton.onClick.AddListener(OnChangeProfileClicked);
		}
		private void OnDisable()
		{
			newGameButton.onClick.RemoveListener(OnNewGameClicked);
			continueGameButton.onClick.RemoveListener(OnContinueGameClicked);
			exitButton.onClick.RemoveListener(OnExitClicked);
			changeProfileButton.onClick.RemoveListener(OnChangeProfileClicked);
		}

		private void OnChangeProfileClicked()
		{
			ChangeProfileWindowViewModel changeProfileWindowViewModel = new ChangeProfileWindowViewModel(_progressDataHandler);
			_disposables.Add(changeProfileWindowViewModel.CurrentUser.Subscribe(user =>
			{
				_viewModel.ChangeUser(user);
			}));
			_uiRootViewModel.OpenWindow(changeProfileWindowViewModel);
		}

		private void OnExitClicked()
		{
			Application.Quit();
		}

		private void OnContinueGameClicked()
		{
			UserProfileData profileData = _viewModel.CurrentUser.CurrentValue;
			profileData.isCurrentUser = true;
			
			_viewModel.StartGame.Invoke();
		}

		private void OnNewGameClicked()
		{
			UserProfileData profileData = _viewModel.CurrentUser.CurrentValue;
			
			profileData.hasSession = true;
			profileData.playerData = new PlayerData();
			profileData.isCurrentUser = true;
			
			_viewModel.StartGame.Invoke();
		}
	}
}
