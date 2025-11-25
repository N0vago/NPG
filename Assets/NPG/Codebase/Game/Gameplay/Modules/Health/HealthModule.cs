using System;
using NPG.Codebase.Infrastructure.ScriptableObjects;
using NPG.Codebase.Infrastructure.ScriptableObjects.CharactersSO;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.Modules.Health
{
    public class HealthModule : MonoBehaviour, IDamageable
    {
        [SerializeField] private CharacterData characterData;

        private float _currentHealth;

        private bool _isDead = false;

        private bool _isImmortal = false;
        public event Action<float> HealthChanged;
        public event Action Dead;

        private void OnEnable()
        {
            Dead += () => Destroy(gameObject);
        }

        private void OnDisable()
        {
            Dead = null;
        }

        private void Start()
        {
            _currentHealth = characterData.maxHealth;
        }

        public bool TakeDamage(float damage)
        {
            if (_isImmortal || _isDead) return false;
            
            if (_currentHealth > 0)
            {
                _currentHealth -= damage;
                HealthChanged?.Invoke(_currentHealth);
                Debug.Log($"Received damage: {damage}");
                return true;
            }

            _isDead = true;
            Dead?.Invoke();

            return true;
        }
    }
}