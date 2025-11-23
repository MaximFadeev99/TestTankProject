using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BaseBuilding.Tests
{
    public class SpriteLoader
    {
        private readonly Dictionary<AssetReferenceSprite, int> _distributedSprites;

        public SpriteLoader()
        {
            _distributedSprites = new Dictionary<AssetReferenceSprite, int>();
        }

        public async UniTask<Sprite> GetSprite(AssetReferenceSprite spriteReference)
        {
            if (spriteReference == null)
            {
                Debug.LogError($"{nameof(SpriteLoader)}: The received reference {spriteReference.AssetGUID} is null or invalid! " +
                               "Call a programmer!");
                return null;
            }
            

            AssetReferenceSprite targetReference = _distributedSprites
                .FirstOrDefault(kvp => kvp.Key.AssetGUID == spriteReference.AssetGUID).Key;

            if (targetReference == null)
            {
                targetReference = spriteReference;
                await LoadAsset(targetReference);
            }
            
            if (targetReference.Asset == null)
                await UniTask.WaitUntil(() => targetReference.Asset != null);

            Sprite sprite = targetReference.Asset as Sprite;

            if (sprite == null)
            {
                Debug.LogError($"{nameof(SpriteLoader)}: The received reference {spriteReference.AssetGUID} is not for a sprite asset! " +
                               "Call a programmer!");
                RemoveReferenceFromCash(targetReference);
                return null;
            }
            
            _distributedSprites[targetReference]++;
            return sprite;
        }

        public void ReleaseSprite(AssetReferenceSprite spriteReference)
        {
            if (spriteReference == null)
                return;
            
            AssetReferenceSprite targetReference = _distributedSprites
                .FirstOrDefault(kvp => kvp.Key.AssetGUID == spriteReference.AssetGUID).Key;

            if (targetReference == null)
            {
                Debug.LogError($"{nameof(SpriteLoader)}: The received reference {spriteReference.AssetGUID} has not been " +
                               $"loaded by this {nameof(SpriteLoader)} and therefore, can not be released by it! " +
                               "Call a programmer!");
                return;
            }

            _distributedSprites[targetReference]--;
            
            if (_distributedSprites[targetReference] > 0)
                return;
            
            targetReference.ReleaseAsset();
            _distributedSprites.Remove(targetReference);
        }

        public void ReleaseAllDistributedSprites()
        {
            foreach (KeyValuePair<AssetReferenceSprite, int> kvp in _distributedSprites)
                kvp.Key.ReleaseAsset();
            
            _distributedSprites.Clear();
        }

        private async UniTask LoadAsset(AssetReferenceSprite spriteReference)
        {
            _distributedSprites[spriteReference] = 0;
            await spriteReference.LoadAssetAsync<Sprite>().Task;
        }

        private void RemoveReferenceFromCash(AssetReferenceSprite spriteReference)
        {
            if (_distributedSprites.ContainsKey(spriteReference) == false)
                return;
            
            _distributedSprites.Remove(spriteReference);
        }
    }
}
