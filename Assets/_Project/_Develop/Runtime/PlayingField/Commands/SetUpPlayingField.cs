using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestTankProject.Runtime.PlayingField
{
    public readonly struct SetUpPlayingField
    {
        public readonly Vector2Int Size;
        public readonly float SpacingBetweenCards;

        public SetUpPlayingField(Vector2Int size, float spacingBetweenCards)
        {
            Size = size;
            SpacingBetweenCards = spacingBetweenCards;
        }
    }
}
