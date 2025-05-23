using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public float maxHealth;
        public float immortalityTime;
        public float speed;
        public float armor;

    }
}