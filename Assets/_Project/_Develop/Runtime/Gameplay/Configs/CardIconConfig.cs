using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TestTankProject.Runtime.Gameplay
{
    [CreateAssetMenu(fileName = "CardIconConfig_New", menuName = "TestTankProject/CardIconConfig", order = 51)]
    public class CardIconConfig : ScriptableObject
    {
        public bool StartWithMe;
        
        [SerializeField] private List<AssetReferenceSprite> _iconReferences;
        
        public IReadOnlyList<AssetReferenceSprite> IconReferences => _iconReferences;
    }
}
