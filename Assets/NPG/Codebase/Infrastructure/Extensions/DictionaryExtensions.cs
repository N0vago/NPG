using System.Collections.Generic;

namespace NPG.Codebase.Infrastructure.Extensions
{
    public static class DictionaryExtensions
    {
        public static TKey GetKeyByValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value)
        {
            foreach (var pair in dict)
            {
                if (EqualityComparer<TValue>.Default.Equals(pair.Value, value))
                    return pair.Key;
            }

            throw new KeyNotFoundException("Value not found in dictionary.");
        }
        
        public static bool TryGetKeyByValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value, out TKey key)
        {
            foreach (var pair in dict)
            {
                if (EqualityComparer<TValue>.Default.Equals(pair.Value, value))
                {
                    key = pair.Key;
                    return true;
                }
            }

            key = default;
            return false;
        }
    }
}