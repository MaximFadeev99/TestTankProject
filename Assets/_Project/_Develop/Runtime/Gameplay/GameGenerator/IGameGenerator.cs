using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay.GameGeneration
{
    public interface IGameGenerator
    {
        public IReadOnlyList<CardModel> GenerateGame(Vector2Int fieldSize, IReadOnlyList<Sprite> icons, 
            out IReadOnlyList<CardDataForView> cardDataForView);
    }
}
