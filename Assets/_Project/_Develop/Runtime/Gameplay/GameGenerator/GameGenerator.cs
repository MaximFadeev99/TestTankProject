using System.Collections.Generic;
using TestTankProject.Runtime.Utilities;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay.GameGeneration
{
    internal abstract class GameGenerator : IGameGenerator
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

        protected abstract IReadOnlyList<CardModel> GenerateCards(Vector2Int fieldSize, 
            IReadOnlyList<Sprite> icons);

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
