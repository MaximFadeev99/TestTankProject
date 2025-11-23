using System.Collections.Generic;
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
