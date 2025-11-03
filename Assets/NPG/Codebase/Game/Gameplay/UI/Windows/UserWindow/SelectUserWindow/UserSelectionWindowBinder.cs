using UnityEngine;
using NPG.Codebase.Game.Gameplay.UI.Windows;
using UnityEngine.UI;
using Assets.NPG.Codebase.Game.Gameplay.UI.Windows.UserWindow.SelectUserWindow;
using R3;
using ObservableCollections;

namespace Assets.NPG.Codebase.Game.Gameplay.UI.Windows.UserWindow
{
	public class UserSelectionWindowBinder : WindowBinder
	{
		[SerializeField] public Button selectUserButton;
		[SerializeField] public Button createProfileButton;

		[SerializeField] public Transform userListContainer;


		public void Bind(UserSelectionWindowViewModel viewModel)
		{
			_disposables.Add(viewModel.CurrentUser.Subscribe(user =>
			{
				// Update UI based on the current user
				Debug.Log($"Current user changed to: {user?.userName}");
			}));
			_disposables.Add(viewModel.UserProfileDatas.ObserveAdd().Subscribe(addEvent =>
			{
				// Handle user profile added to the list
				Debug.Log($"User profile added: {addEvent.Value.userName}");
			}));
		}
	}
}