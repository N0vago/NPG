using System.Collections.Generic;
using System.Linq;
using NPG.Codebase.Game.Gameplay.Enemy;
using NPG.Codebase.Infrastructure.JsonData;
using NPG.Codebase.Infrastructure.ScriptableObjects.AchievementsSO;
using NPG.Codebase.Infrastructure.ScriptableObjects.StaticData;
using NPG.Codebase.Infrastructure.Services.DataSaving;
using UnityEngine;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.Achievements
{
    public class AchievementManager : MonoBehaviour, IDataWriter
    {
        [SerializeField] private AchievementsTable achievementsTable;
        
        private ProgressDataHandler _progressDataHandler;
        private List<string> _unlockedAchievementIDs;
        
        //Fields for achievements
        private bool _isFirstLaunch = false;
        private int _greenEnemyCount;
        private int _yellowEnemyCount;
        private int _redEnemyCount;
        public static AchievementManager Instance { get; private set; }

        [Inject]
        public void Construct(ProgressDataHandler progressDataHandler)
        {
            _progressDataHandler = progressDataHandler;
            _progressDataHandler.RegisterObserver(this);
        }

        public void FirstLaunch()
        {
            if (!_isFirstLaunch)
            {
                _isFirstLaunch = true;
                UnlockAchievements("a_1");
            }
        }

        public void IncreaseEnemyCount(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Green:
                    _greenEnemyCount++;
                    break;
                case EnemyType.Yellow:
                    _yellowEnemyCount++;
                    break;
                case EnemyType.Red:
                    _redEnemyCount++;
                    break;
            }

            if (_greenEnemyCount > 0 || _yellowEnemyCount > 0 || _redEnemyCount > 0)
            {
                UnlockAchievements("a_2");
            }
            else if (_greenEnemyCount >= 5)
            {
                UnlockAchievements("a_3");
            }
            else if (_greenEnemyCount >= 10)
            {
                UnlockAchievements("a_4");
            }
            else if (_yellowEnemyCount > 0)
            {
                UnlockAchievements("a_5");
            }
            else if (_yellowEnemyCount >= 5)
            {
                UnlockAchievements("a_6");
            }
            else if (_yellowEnemyCount >= 10)
            {
                UnlockAchievements("a_7");
            }
            else if (_redEnemyCount > 0)
            {
                UnlockAchievements("a_8");
            }
            else if (_redEnemyCount >= 5)
            {
                UnlockAchievements("a_9");
            }
            else if (_redEnemyCount >= 10)
            {
                UnlockAchievements("a_10");
            }
            
        }
        
        private void UnlockAchievements(string achievementID)
        {
            if(_unlockedAchievementIDs.Contains(achievementID)) return;
            _unlockedAchievementIDs.Add(achievementID);
            AchievementStaticData staticData = achievementsTable.achievements.FirstOrDefault(data => data.ID == achievementID);
            if (staticData != null) Debug.Log("UnlockAchievements: " + staticData.Description);
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            }
            DontDestroyOnLoad(this);
        }

        public void Load(GameData data)
        {
            string userID = data.currentUserId;
            int.TryParse(userID, out int id);
            _unlockedAchievementIDs = data.userData[id].achievementsIDs;
            _isFirstLaunch = data.userData[id].playerData.playerStatistics.firstLaunch;
            _greenEnemyCount = data.userData[id].playerData.playerStatistics.greenEnemiesCount;
            _yellowEnemyCount = data.userData[id].playerData.playerStatistics.yellowEnemiesCount;
            _redEnemyCount = data.userData[id].playerData.playerStatistics.redEnemiesCount;
        }

        public void Save(ref GameData data)
        {
            string userID = data.currentUserId;
            int.TryParse(userID, out int id);
            data.userData[id].achievementsIDs = _unlockedAchievementIDs;
            data.userData[id].playerData.playerStatistics.firstLaunch = _isFirstLaunch;
            data.userData[id].playerData.playerStatistics.greenEnemiesCount = _greenEnemyCount;
            data.userData[id].playerData.playerStatistics.yellowEnemiesCount = _yellowEnemyCount;
            data.userData[id].playerData.playerStatistics.redEnemiesCount = _redEnemyCount;
        }
    }
}