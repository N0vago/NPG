using System.Linq;
using NPG.Codebase.Game.Gameplay.UI.Windows;
using NPG.Codebase.Infrastructure.JsonData;
using NPG.Codebase.Infrastructure.Services.DataSaving;
using ObservableCollections;
using R3;

namespace NPG.Codebase.Game.Gameplay.UI.Menu
{
    public class ChangeProfileWindowViewModel : WindowViewModel, IDataWriter
    {
        private ReactiveProperty<UserProfileData> _currentUser = new();
        
        private ObservableList<UserProfileData> _users = new();

        public ReadOnlyReactiveProperty<UserProfileData> CurrentUser => _currentUser;
        public IObservableCollection<UserProfileData> Users => _users;
        
        public override string Id =>  "ChangeProfileWindow";
        
        private ProgressDataHandler _progressDataHandler;

        public ChangeProfileWindowViewModel(ProgressDataHandler progressDataHandler)
        {
            _progressDataHandler = progressDataHandler;
            progressDataHandler.RegisterObserver(this);
        }

        public void AddUser(UserProfileData user)
        {
            _users.Add(user);
            _progressDataHandler.SaveProgress(this);
        }
        public void SetUser(UserProfileData user)
        {
            _currentUser.Value = user;
        }
        
        public void Load(GameData data)
        {
            _currentUser.Value = data.userData[data.GetCurrentUserIndex()];
            _users.AddRange(data.userData);
        }

        public void Save(ref GameData data)
        {
            data.currentUserId = _currentUser.Value.userId;
            foreach (var user in _users)
            {
                if (data.userData.Any(x => x.userId == user.userId))
                {
                    continue;
                }
                data.userData.Add(user);
            }
        }
    }
}