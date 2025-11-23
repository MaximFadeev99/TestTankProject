using System.Collections;
using System.Collections.Generic;
using TestTankProject.Runtime.Bootstrap;
using UnityEngine;

namespace TestTankProject.Runtime.PlayingField
{
    [Message]
    public readonly struct CardClickedEvent
    {
        public readonly Vector2Int CardAddress;

        public CardClickedEvent(Vector2Int cardAddress)
        {
            CardAddress = cardAddress;
        }
    }
}
