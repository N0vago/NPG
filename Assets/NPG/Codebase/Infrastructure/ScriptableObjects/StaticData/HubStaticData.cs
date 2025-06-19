using System;
using UnityEngine.AddressableAssets;

namespace NPG.Codebase.Infrastructure.ScriptableObjects.StaticData
{
    [Serializable]
    public class HubStaticData
    {
        public HubIDs hubID;
        public string addressableName;
    }

    public enum HubIDs
    {
        Unknown,
        Player,
        CinemachineCamera,
        Camera,
        UIRoot
    }
}