using UnityEngine;

namespace NPG.Codebase.Infrastructure.ScriptableObjects.CharactersSO
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