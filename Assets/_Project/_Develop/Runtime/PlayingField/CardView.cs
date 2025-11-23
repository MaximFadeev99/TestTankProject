using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TestTankProject.Runtime.PlayingField
{
    internal class CardView : MonoBehaviour
    {
        private const float AnimDuratiton = 0.5f;
        
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private SpriteRenderer _icon;
        [SerializeField] private Transform _coverTransform;
        [SerializeField] private BoxCollider2D _coverCollider;
        
        private GameObject _gameObject;
        private Transform _transform;
        private Vector3 _initialCoverTransformScale;
        private Tween _raiseCoverTween;
        private Tween _putDownCoverTween;
        
        
        internal float HalfWidth => _background.bounds.extents.x;
        internal float HalfHeight => _background.bounds.extents.y;
        internal Vector2 WorldPosition => _transform.position;
        internal Vector2Int Address { get; private set; }

        internal Action<Vector2Int> Pressed;
        
        internal void Initialize(string newName, Vector2Int address, Sprite icon)
        {
            _gameObject = gameObject;
            _gameObject.name = newName;
            _transform = transform;
            Address = address;
            _icon.sprite = icon;
            _initialCoverTransformScale = _coverTransform.localScale;
        }

        internal void SetLocalPosition(Vector2 localPosition)
        {
            _transform.localPosition = localPosition;
        }

        internal void RaiseCover()
        {
            _putDownCoverTween?.Kill(false);

            if (_gameObject.activeSelf == false || _raiseCoverTween != null)
                return;
            
            _raiseCoverTween = _coverTransform
                .DOScale(Vector3.zero, AnimDuratiton)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    _raiseCoverTween = null;
                    _coverCollider.enabled = false;
                });
        }

        internal void PutDownCover()
        {
            _raiseCoverTween?.Kill();

            if (_gameObject.activeSelf == false || _putDownCoverTween != null)
                return;
            
            _putDownCoverTween = _coverTransform
                .DOScale(_initialCoverTransformScale, AnimDuratiton)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    _putDownCoverTween = null;
                    _coverCollider.enabled = true;
                });
        }

        internal async void Remove(bool shallPlayAnimation = true)
        {
            if (shallPlayAnimation)
            {
                _coverTransform.DOKill();
                _coverTransform.gameObject.SetActive(false);
                await _transform.DOScale(Vector3.zero, AnimDuratiton).SetEase(Ease.InBack);
            }
            
            _gameObject.SetActive(false);
            _coverCollider.enabled = false;
        }

        internal void OnHitByRaycast()
        {
            //_coverCollider.enabled = false;
            Pressed?.Invoke(Address);
        }

        internal void Destroy()
        {
            _raiseCoverTween?.Kill();
            _putDownCoverTween?.Kill();
            
            if (_transform != null)
                _transform.DOKill();
        }
    }
}
