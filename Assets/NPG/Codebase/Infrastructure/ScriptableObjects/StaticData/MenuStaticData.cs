using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPG.Codebase.Infrastructure.ScriptableObjects.StaticData
{
	[Serializable]
	public class MenuStaticData
	{
		public MenuIDs menuID;
		public string addressableName;
	}
	public enum MenuIDs
	{
		MenuCanvas,
		UserWindow,
		UIRoot

	}
}
