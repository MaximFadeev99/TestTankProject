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
        public IReadOnlyList<CardModel> GenerateGame(Vector2Int fieldSize,
            IReadOnlyList<AssetReferenceSprite> iconReferences)
        {
            if (iconReferences.Count < fieldSize.x * fieldSize.y / RuntimeConstants.MatchingCardCount)
            {
                CustomLogger.Log($"{nameof(OrderedGameGeneration)}","FAILED to generate cards since icons are not enough to match the firld size " +
                                 $"{fieldSize.x * fieldSize.y}", MessageTypes.Error, RecipientTypes.GD);
            }
            
            return GenerateCards(fieldSize, iconReferences);
        }
        

        protected abstract IReadOnlyList<CardModel> GenerateCards(Vector2Int fieldSize, 
            IReadOnlyList<AssetReferenceSprite> iconReferences);
    }
}
