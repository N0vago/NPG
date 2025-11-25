using System.Collections.Generic;
using NPG.Codebase.Infrastructure.ScriptableObjects.StaticData;
using UnityEngine;

namespace NPG.Codebase.Infrastructure.ScriptableObjects.AchievementsSO
{
	[CreateAssetMenu(fileName = "AchievementsTable", menuName = "ScriptableObjects/AchievementsTable", order = 0)]
	public class AchievementsTable : ScriptableObject
	{
		public List<AchievementStaticData> achievements;
	}
}
