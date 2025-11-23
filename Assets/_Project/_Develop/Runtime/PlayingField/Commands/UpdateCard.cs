using TestTankProject.Runtime.Bootstrap;
using UnityEngine;

namespace TestTankProject.Runtime.PlayingField
{
    [Message]
    public readonly struct UpdateCard
    {
        public readonly Vector2Int CardAddress;
        public readonly CardActions Action;

        public UpdateCard(Vector2Int cardAddress, CardActions action)
        {
            CardAddress = cardAddress;
            Action = action;
        }
    }
}