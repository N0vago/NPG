using System;

namespace NPG.Codebase.Infrastructure.ScriptableObjects.StaticData
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
