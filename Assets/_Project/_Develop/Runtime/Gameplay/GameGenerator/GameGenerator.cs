using System.Collections.Generic;
using BaseBuilding.Tests;
using Cysharp.Threading.Tasks;
using TestTankProject.Runtime.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TestTankProject.Runtime.Gameplay.GameGeneration
{
    internal abstract class GameGenerator : IGameGenerator
    {
        public async UniTask<GameGenerationResult> GenerateGame(Vector2Int fieldSize, SpriteLoader spriteLoader,
            IReadOnlyList<AssetReferenceSprite> iconReferences)
        {
            if (iconReferences.Count < fieldSize.x * fieldSize.y / RuntimeConstants.MatchingCardCount)
            {
                CustomLogger.Log($"{nameof(OrderedGameGeneration)}","FAILED to generate cards since icons are not enough to match the firld size " +
                                 $"{fieldSize.x * fieldSize.y}", MessageTypes.Error, RecipientTypes.GD);
            }

            IReadOnlyList<CardModel> cards = GenerateCards(fieldSize, iconReferences);
            IReadOnlyList<CardDataForView> cardsForView = await GenerateCardDataForView(cards, spriteLoader);
            
            return new GameGenerationResult(cards, cardsForView);
        }

        protected abstract IReadOnlyList<CardModel> GenerateCards(Vector2Int fieldSize, 
            IReadOnlyList<AssetReferenceSprite> iconReferences);

        private async UniTask<IReadOnlyList<CardDataForView>> GenerateCardDataForView(IReadOnlyList<CardModel> cards, 
            SpriteLoader spriteLoader)
        {
            List<CardDataForView> cardsForView = new();

            foreach (CardModel card in cards)
            {
                Sprite icon = await spriteLoader.GetSprite(card.IconReference);
                cardsForView.Add(new CardDataForView(card.Address, icon));
            }
            
            return cardsForView;
        }
    }

    public readonly struct GameGenerationResult
    {
        public readonly IReadOnlyList<CardModel> Cards;
        public readonly IReadOnlyList<CardDataForView> CardsForView;

        public GameGenerationResult(IReadOnlyList<CardModel> cards, 
            IReadOnlyList<CardDataForView> cardsForView)
        {
            Cards = cards;
            CardsForView = cardsForView;
        }
    }
}
