using NPG.Codebase.Game.Gameplay.Achievements;
using NPG.Codebase.Game.Gameplay.Modules.Health;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.Enemy
{
    [RequireComponent(typeof(HealthModule))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyType  _enemyType;
        private HealthModule _healthModule;
        
        private void Awake()
        {
            _healthModule = GetComponent<HealthModule>();
            _healthModule.Dead += () => AchievementManager.Instance.IncreaseEnemyCount(_enemyType);
        }
        
    }

    public enum EnemyType
    {
        Green,
        Yellow,
        Red
    }
}