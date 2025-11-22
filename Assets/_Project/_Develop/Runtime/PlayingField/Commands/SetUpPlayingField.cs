using System.Collections;
using System.Collections.Generic;
using TestTankProject.Runtime.Gameplay;
using UnityEngine;

namespace TestTankProject.Runtime.PlayingField
{
    public readonly struct SetUpPlayingField
    {
        public readonly Vector2Int Size;
        public readonly float SpacingBetweenCards;
        public readonly IReadOnlyList<CardDataForView> CardData;

        public SetUpPlayingField(Vector2Int size, float spacingBetweenCards, 
            IReadOnlyList<CardDataForView> cardData)
        {
            Size = size;
            SpacingBetweenCards = spacingBetweenCards;
            CardData = cardData;
        }
    }
}
