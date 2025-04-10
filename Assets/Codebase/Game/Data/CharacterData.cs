using UnityEngine;

namespace Codebase.Game.Data
{
    [CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        //General params
        [HideInInspector]
        public bool isDead;
        [HideInInspector] 
        public bool isImmortal;
        [HideInInspector]
        public float currentHealth;
        
        public float maxHealth;
        public float immortalityTime;
        public float speed;
        public float armor;

    }
}