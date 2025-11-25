using System;
using System.Collections.Generic;

namespace NPG.Codebase.Infrastructure.JsonData
{
	[Serializable]
	public class UserProfileData
	{
		public string userName;
		public string userId;
		public string userIconPath;

		public bool isCurrentUser;

		public List<string> achievementsIDs;

		public PlayerData playerData = new();	
	}
}
