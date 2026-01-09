using Random = UnityEngine.Random;

namespace NPG.Codebase.Utils
{
    public static class IDGenerator
    {
        public static string GenerateProfileID()
        {
            int id = Random.Range(0, 100);
            return id.ToString();
        }
    }
}