using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Infrastructure.JsonData;
using R3;

namespace NPG.Codebase.Game.Gameplay.UI.Menu
{
	public class MenuViewModel : ViewModel
	{
		private readonly ReactiveProperty<UserProfileData> _currentUser;

		public ReadOnlyReactiveProperty<UserProfileData> CurrentUser => _currentUser;

		public MenuViewModel(UserProfileData initialUser)
		{
			_currentUser = new ReactiveProperty<UserProfileData>(initialUser);
		}

		public void ChangeUser(UserProfileData newUser)
		{
			_currentUser.Value = newUser;
		}
	}
}
