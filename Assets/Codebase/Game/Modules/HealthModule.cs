using System;
using Codebase.Game.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Codebase.Game.Modules
{
    public class HealthModule : MonoBehaviour
    {
        [SerializeField] private CharacterData characterData;
        
        public event Action<GameObject> Dead;

        private void OnEnable()
        {
            Dead += Destroy;
        }

        private void OnDisable()
        {
            Dead -= Destroy;
        }

        private void Start()
        {
            characterData.isImmortal = false;
            characterData.isDead = false;
            characterData.currentHealth = characterData.maxHealth;
        }

        public async UniTask<bool> ReceiveDamage(float damage)
        {
            if (characterData.isImmortal || characterData.isDead) return false;
            
            characterData.currentHealth -= damage;
            Debug.Log($"Received damage: {damage}");
            
            if (characterData.currentHealth > 0)
            {
                characterData.isImmortal = true;

                await UniTask.WaitForSeconds(characterData.immortalityTime);

                characterData.isImmortal = false;

                return true;
            }

            characterData.isDead = true;
            Dead?.Invoke(gameObject);

            return true;
        }
    }
}