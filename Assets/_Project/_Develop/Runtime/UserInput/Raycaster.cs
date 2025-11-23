using System;
using MessagePipe;
using TestTankProject.Runtime.PlayingField;
using UnityEngine;

namespace TestTankProject.Runtime.UserInput
{
    public class Raycaster : IDisposable
    {
        private readonly Camera _mainCamera;
        private readonly LayerMask _cardLayerMask;
        private readonly IDisposable _disposableForSubscriptions;

        public Raycaster(Camera mainCamera, ISubscriber<UserClickRegisteredEvent> userClickSubscriber)
        { 
            _mainCamera = mainCamera;
            _cardLayerMask = RuntimeConstants.CardsLayerMask;
            
            DisposableBagBuilder bagBuilder = DisposableBag.CreateBuilder();
            userClickSubscriber.Subscribe(OnUserClick).AddTo(bagBuilder);
            _disposableForSubscriptions = bagBuilder.Build();
        }

        private void OnUserClick(UserClickRegisteredEvent userClickEvent)
        {
            Vector2 worldPosition = _mainCamera.ScreenToWorldPoint(userClickEvent.ClickScreenPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, 
                10f, _cardLayerMask);
            
            if (hit.collider == null)
                return;
            
            CardView hitCard = hit.transform.parent.GetComponent<CardView>();
            hitCard.OnHitByRaycast();
        }

        public void Dispose()
        {
            _disposableForSubscriptions?.Dispose();
        }
    }
}
