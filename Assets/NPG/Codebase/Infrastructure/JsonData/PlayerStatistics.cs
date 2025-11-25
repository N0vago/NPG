using System;

namespace NPG.Codebase.Infrastructure.JsonData
{
    [Serializable]
    public class PlayerStatistics
    {
        public bool firstLaunch;
        public int greenEnemiesCount;
        public int yellowEnemiesCount;
        public int redEnemiesCount;
    }
}