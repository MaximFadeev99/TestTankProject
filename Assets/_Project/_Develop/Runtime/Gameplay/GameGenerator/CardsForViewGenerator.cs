using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TestTankProject.Runtime.AssetLoading;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay.GameGeneration
{
    internal class CardsForViewGenerator
    {
        private readonly SpriteLoader _spriteLoader;

        public CardsForViewGenerator(SpriteLoader spriteLoader)
        {
            _spriteLoader = spriteLoader;
        }

        internal async UniTask<IReadOnlyList<CardDataForView>> Generate(IReadOnlyList<CardModel> cards)
        {
            List<CardDataForView> cardsForView = new();

            foreach (CardModel card in cards)
            {
                Sprite icon = await _spriteLoader.GetSprite(card.IconReference);
                cardsForView.Add(new(card.Address, icon, card.Status == CardStatus.Unmatched));
            }
            
            return cardsForView;
        }
    }
}