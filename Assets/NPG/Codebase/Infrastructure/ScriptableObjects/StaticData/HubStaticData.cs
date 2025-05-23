using System;
using UnityEngine.AddressableAssets;

namespace NPG.Codebase.Infrastructure.ScriptableObjects.StaticData
{
    [Serializable]
    public class HubStaticData
    {
        public HubIDs hubID;
        public AssetReference prefab;
    }

    public enum HubIDs
    {
        Unknown,
        Player,
        CinemachineCamera,
        Camera
    }
}