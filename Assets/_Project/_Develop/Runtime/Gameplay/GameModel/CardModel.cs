using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay
{
    public class CardModel
    {
        internal readonly Vector2Int Address;
        internal readonly Vector2Int MatchingCardAddress;
        internal readonly Sprite IconKey;

        internal CardStatus Status;

        public CardModel(Vector2Int address, Vector2Int matchingCardAddress, Sprite iconKey)
        {
            Address = address;
            MatchingCardAddress = matchingCardAddress;
            IconKey = iconKey;
            Status = CardStatus.Unmatched;
        }
    }

    internal enum CardStatus
    {
        Undefined = 0,
        Unmatched,
        Matched
    }
}
