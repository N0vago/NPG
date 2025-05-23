using System.Collections.Generic;
using NPG.Codebase.Infrastructure.ScriptableObjects.StaticData;
using UnityEngine;

namespace NPG.Codebase.Infrastructure.ScriptableObjects
{
    [CreateAssetMenu(fileName = "HubObjects", menuName = "ScriptableObjects/HubObjects", order = 1)]
    public class HubObjects : ScriptableObject
    {
        public List<HubStaticData> hubObjects;
    }
}