using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestTankProject.Runtime.PlayingField
{
    internal class CardView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private Transform _transform;
        
        internal float HalfWidth => _spriteRenderer.bounds.extents.x;
        internal float HalfHeight => _spriteRenderer.bounds.extents.y;
        internal Vector2 WorldPosition => _transform.position;

        internal void Initialize(string newName)
        {
            gameObject.name = newName;
            _transform = transform;
        }

        internal void SetLocalPosition(Vector2 localPosition)
        {
            _transform.localPosition = localPosition;
        }

        internal void Destroy()
        {
            
        }
    }
}
