using System.Collections.Generic;
using System.Linq;
using TestTankProject.Runtime.Utilities;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay.GameGeneration
{
    internal class RandomGameGenerator : IGameGenerator
    {
        public IReadOnlyList<CardModel> GenerateGame(Vector2Int fieldSize, IReadOnlyList<Sprite> icons, 
            out IReadOnlyList<CardDataForView> cardDataForView)
        {
            if (icons.Count < fieldSize.x * fieldSize.y / RuntimeConstants.MatchingCardCount)
            {
                CustomLogger.Log($"{nameof(OrderedGameGeneration)}","FAILED to generate cards since icons are not enough to match the firld size " +
                                 $"{fieldSize.x * fieldSize.y}", MessageTypes.Error, RecipientTypes.GD);
            }

            IReadOnlyList<CardModel> cards = GenerateCards(fieldSize, icons);
            cardDataForView = GenerateCardDataForView(cards);
            
            return cards;
        }

        private IReadOnlyList<CardModel> GenerateCards(Vector2Int fieldSize, IReadOnlyList<Sprite> icons)
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
                    icons[iconIndex]));
                cards.Add(new CardModel(allAddresses[randomAddressIndex], allAddresses[0],
                    icons[iconIndex]));
                iconIndex++;
                allAddresses.RemoveAt(randomAddressIndex);
                allAddresses.RemoveAt(0);
            }
            
            cards = cards.OrderBy(card => card.Address.x + card.Address.y).ToList();
            
            return cards;
        }

        private IReadOnlyList<CardDataForView> GenerateCardDataForView(IReadOnlyList<CardModel> cards)
        {
            List<CardDataForView> cardsForView = new();

            foreach (CardModel card in cards)
            {
                cardsForView.Add(new CardDataForView(card.Address, card.IconKey));
            }
            
            return cardsForView;
        }
    }
}
