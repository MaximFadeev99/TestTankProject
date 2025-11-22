using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay.CardGeneration
{
    public interface ICardGenerator
    {
        public IReadOnlyList<CardModel> GenerateCards(Vector2Int fieldSize, IReadOnlyList<Sprite> icons, 
            out IReadOnlyList<CardDataForView> cardDataForView);
    }
}
