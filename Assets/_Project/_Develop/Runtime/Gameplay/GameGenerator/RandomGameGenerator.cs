using System.Collections.Generic;
using System.Linq;
using BaseBuilding.Tests;
using TestTankProject.Runtime.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TestTankProject.Runtime.Gameplay.GameGeneration
{
    internal class RandomGameGenerator : GameGenerator
    {
        protected override IReadOnlyList<CardModel> GenerateCards(Vector2Int fieldSize,
            IReadOnlyList<AssetReferenceSprite> iconReferences)
        {
            if (fieldSize.x * fieldSize.y % RuntimeConstants.MatchingCardCount != 0)
            {
                CustomLogger.Log(nameof(GameConfig), $"You can't have a playing field {fieldSize.x} by {fieldSize.y}, " +
                                                     $"because then there will be uneven card number!", 
                    MessageTypes.Error, RecipientTypes.GD);
            }
            
            List<CardModel> cards = new();
            List<Vector2Int> allAddresses = new();
            
            for (int i = 0; i < fieldSize.x; i++)
            {
                for (int j = 0; j < fieldSize.y; j++)
                {
                    allAddresses.Add(new(i, j));
                }
            }
            
            int iconIndex = 0;

            while (allAddresses.Count != 0)
            {
                int randomAddressIndex = Random.Range(1, allAddresses.Count);
                cards.Add(new CardModel(allAddresses[0], allAddresses[randomAddressIndex], 
                    iconReferences[iconIndex]));
                cards.Add(new CardModel(allAddresses[randomAddressIndex], allAddresses[0],
                    iconReferences[iconIndex]));
                iconIndex++;
                allAddresses.RemoveAt(randomAddressIndex);
                allAddresses.RemoveAt(0);
            }
            
            cards = cards.OrderBy(card => card.Address.x + card.Address.y).ToList();
            
            return cards;
        }
    }
}
