using System;
using System.Collections.Generic;

namespace NPG.Codebase.Infrastructure.JsonData
{
    [Serializable]
    public class GameData
    {
        public List<UserProfileData> userData = new();
        public string currentUserId;

        public GamePreferences gamePreferences = new();

        public int GetCurrentUserIndex()
        {
            return userData.FindIndex(user => user.userId == currentUserId);
		}

	}
}