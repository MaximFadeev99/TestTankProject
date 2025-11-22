using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TestTankProject.Runtime.PlayingField
{
    internal class CardView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private SpriteRenderer _icon;
        [SerializeField] private Transform _coverTransform;
        
        private Transform _transform;
        private Vector2Int _address;
        private Vector3 _initialCoverTransformScale;
        
        internal float HalfWidth => _background.bounds.extents.x;
        internal float HalfHeight => _background.bounds.extents.y;
        internal Vector2 WorldPosition => _transform.position;

        internal Action<Vector2Int> Pressed;
        
        internal void Initialize(string newName, Vector2Int address, Sprite icon)
        {
            gameObject.name = newName;
            _transform = transform;
            _address = address;
            _icon.sprite = icon;
            _initialCoverTransformScale = _coverTransform.localScale;
        }

        internal void SetLocalPosition(Vector2 localPosition)
        {
            _transform.localPosition = localPosition;
        }

        internal void RaiseCover()
        {
            _coverTransform.DOKill();
            _coverTransform.DOScale(Vector3.zero, 0.6f).SetEase(Ease.InBack);
        }

        internal void PutDownCover()
        {
            _coverTransform.DOKill();
            _coverTransform.DOScale(_initialCoverTransformScale, 0.6f).SetEase(Ease.OutBack);
        }

        internal void OnHitByRaycast()
        {
            Pressed?.Invoke(_address);
        }

        internal void Destroy()
        {
            _coverTransform.DOKill();
        }
    }
}
