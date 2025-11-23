using Newtonsoft.Json;
using TestTankProject.Runtime.Core.SaveLoad;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TestTankProject.Runtime.Gameplay
{
    [JsonConverter(typeof(CardModelConverter))]
    public class CardModel
    {
        internal readonly Vector2Int Address;
        internal readonly Vector2Int MatchingCardAddress;
        internal readonly AssetReferenceSprite IconReference;

        internal CardStatus Status;

        public CardModel(Vector2Int address, Vector2Int matchingCardAddress, 
            AssetReferenceSprite iconReference, CardStatus status = CardStatus.Unmatched)
        {
            Address = address;
            MatchingCardAddress = matchingCardAddress;
            IconReference = iconReference;
            Status = status;
        }
    }

    public enum CardStatus
    {
        Undefined = 0,
        Unmatched,
        Matched
    }
}
