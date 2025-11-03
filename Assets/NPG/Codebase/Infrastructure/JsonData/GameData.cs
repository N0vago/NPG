using Assets.NPG.Codebase.Infrastructure.JsonData;
using System;
using System.Collections.Generic;

namespace NPG.Codebase.Infrastructure.JsonData
{
    [Serializable]
    public class GameData
    {
        public List<Assets.NPG.Codebase.Infrastructure.JsonData.UserProfileData> userData = new();
        public string currentUserId;

        public int GetCurrentUserIndex()
        {
            return userData.FindIndex(user => user.userId == currentUserId);
		}

	}
}