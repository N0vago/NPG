using NPG.Codebase.Infrastructure.JsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPG.Codebase.Infrastructure.JsonData
{
	[Serializable]
	public class UserProfileData
	{
		public string userName;
		public string userId;
		public string userIconPath;

		public bool isCurrentUser;

		public PlayerData playerData = new();	
	}
}
