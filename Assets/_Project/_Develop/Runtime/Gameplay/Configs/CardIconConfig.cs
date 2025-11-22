using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TestTankProject.Runtime.Gameplay
{
    [CreateAssetMenu(fileName = "CardIconConfig_New", menuName = "TestTankProject/CardIconConfig", order = 51)]
    public class CardIconConfig : ScriptableObject
    {
        [FormerlySerializedAs("ShallUseMe")] public bool StartWithMe;
        
        [SerializeField] private List<Sprite> _cardIcons;
        
        public IReadOnlyList<Sprite> CardIcons => _cardIcons;
    }
}
