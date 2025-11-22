using System;
using UnityEngine;

namespace TestTankProject.Runtime.PlayingField
{
    internal class CardView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private SpriteRenderer _icon;
        
        private Transform _transform;
        private Vector2Int _address;
        
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
        }

        internal void SetLocalPosition(Vector2 localPosition)
        {
            _transform.localPosition = localPosition;
        }

        internal void OnHitByRaycast()
        {
            Pressed?.Invoke(_address);
        }

        internal void Destroy()
        {
            
        }
    }
}
