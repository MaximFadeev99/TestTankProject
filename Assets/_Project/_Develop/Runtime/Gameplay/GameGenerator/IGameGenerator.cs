using System.Collections.Generic;
using BaseBuilding.Tests;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TestTankProject.Runtime.Gameplay.GameGeneration
{
    public interface IGameGenerator
    {
        public UniTask<GameGenerationResult> GenerateGame(Vector2Int fieldSize, SpriteLoader spriteLoader,
            IReadOnlyList<AssetReferenceSprite> iconReferences);
    }
}
