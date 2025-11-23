using System.Collections.Generic;
using BaseBuilding.Tests;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TestTankProject.Runtime.Gameplay.GameGeneration
{
    public interface IGameGenerator
    {
        public IReadOnlyList<CardModel> GenerateGame(Vector2Int fieldSize,
            IReadOnlyList<AssetReferenceSprite> iconReferences);
    }
}
