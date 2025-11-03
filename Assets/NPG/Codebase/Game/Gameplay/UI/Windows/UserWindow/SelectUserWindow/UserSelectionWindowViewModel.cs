using Assets.NPG.Codebase.Infrastructure.JsonData;
using NPG.Codebase.Game.Gameplay.UI.Windows;
using NPG.Codebase.Infrastructure.IDs;
using NPG.Codebase.Infrastructure.JsonData;
using NPG.Codebase.Infrastructure.Services.DataSaving;
using ObservableCollections;
using R3;

namespace Assets.NPG.Codebase.Game.Gameplay.UI.Windows.UserWindow.SelectUserWindow
{
	public class UserSelectionWindowViewModel : WindowViewModel, IDataReader
	{
		private readonly ReactiveProperty<UserProfileData> _currentUser;

		private readonly ObservableList<UserProfileData> userProfileDatas;

		public ReadOnlyReactiveProperty<UserProfileData> CurrentUser => _currentUser;
		public IObservableCollection<UserProfileData> UserProfileDatas => userProfileDatas;
		public override string Id => WindowIDs.UserSelection;

		public UserSelectionWindowViewModel(ProgressDataHandler dataHandler)
		{
			_currentUser = new ReactiveProperty<UserProfileData>();
			userProfileDatas = new ObservableList<UserProfileData>();
			dataHandler.RegisterObserver(this);
		}

		public void Load(GameData data)
		{
			
			_currentUser.Value = data.userData[data.GetCurrentUserIndex()];

			userProfileDatas.Clear();
			foreach (var user in data.userData)
			{
				userProfileDatas.Add(user);
			}
		}
	}
}
