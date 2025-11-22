using UnityEngine;

namespace TestTankProject.Runtime.Gameplay
{
    [CreateAssetMenu(fileName = "GameConfig_New", menuName = "TestTankProject/GameConfig", order = 51)]
    public class GameConfig : ScriptableObject
    {
        public bool StartGameWithMe = false;
        public Vector2Int PlayingFieldSize = new Vector2Int(4,4);
        public float InitialCardDemonstrationTime = 3f;
    }
}
