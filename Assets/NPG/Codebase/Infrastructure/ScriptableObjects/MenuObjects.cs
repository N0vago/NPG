using System.Collections.Generic;
using UnityEngine;

namespace Assets.NPG.Codebase.Infrastructure.ScriptableObjects
{
	[CreateAssetMenu(fileName = "MenuObjects", menuName = "ScriptableObjects/MenuObjects", order = 1)]
	public class MenuObjects : ScriptableObject
	{
		public List<StaticData.MenuStaticData> Objects;
	}
}
