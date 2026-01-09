using System;
using NPG.Codebase.Game.Gameplay.UI.Windows;
using NPG.Codebase.Infrastructure.JsonData;
using R3;

namespace NPG.Codebase.Game.Gameplay.UI.Menu
{
    public class CreateProfileWindowViewModel : WindowViewModel
    {
        private readonly Subject<UserProfileData> _newProfile =  new();

        public Observable<UserProfileData> NewProfile => _newProfile;
        public override string Id => "CreateProfileWindow";

        private UserProfileData[] _existingProfiles;

        public CreateProfileWindowViewModel(UserProfileData[] existingProfiles)
        {
            _existingProfiles = existingProfiles;
        }

        public bool TryCreateProfile(UserProfileData profile)
        {
            foreach (var existingProfile in _existingProfiles)
            {
                if(existingProfile.userId == profile.userId)
                    return false;
            }

            _newProfile.OnNext(profile);
            return true;
        }
    }
}