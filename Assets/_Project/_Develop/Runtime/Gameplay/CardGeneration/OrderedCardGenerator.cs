using System.Collections;
using System.Collections.Generic;
using TestTankProject.Runtime.Utilities;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay.CardGeneration
{
    internal class OrderedGameGeneration : ICardGenerator
    {
        public IReadOnlyList<CardModel> GenerateCards(Vector2Int fieldSize, IReadOnlyList<Sprite> icons, 
            out IReadOnlyList<CardDataForView> cardDataForView)
        {
            if (icons.Count < fieldSize.x * fieldSize.y / RuntimeConstants.MatchingCardCount)
            {
                CustomLogger.Log($"{nameof(OrderedGameGeneration)}","FAILED to generate cards since icons are not enough to match the firld size " +
                                 $"{fieldSize.x * fieldSize.y}", MessageTypes.Error, RecipientTypes.GD);
            }

            List<CardDataForView> tempCardDataForViews = new List<CardDataForView>();

            int iconIndex = 0;
            int iconUseCount = 0;
            
            for (int i = 0; i < fieldSize.x; i++)
            {
                for (int j = 0; j < fieldSize.y; j++)
                {
                    tempCardDataForViews.Add(new CardDataForView(new Vector2Int(i, j), 
                        icons[iconIndex]));
                    iconUseCount++;

                    if (iconUseCount != RuntimeConstants.MatchingCardCount) 
                        continue;
                    
                    iconUseCount = 0;
                    iconIndex++;
                }
            }
            
            cardDataForView = tempCardDataForViews;
            return new List<CardModel>();
        }
    }
}
