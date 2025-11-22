using System;
using System.Collections.Generic;
using MessagePipe;
using TestTankProject.Runtime.MainMenu;
using TestTankProject.Runtime.UI.MainMenu;
using UnityEngine;
using VContainer;

namespace TestTankProject.Runtime.PlayingField
{
    public class PlayingFieldView : MonoBehaviour
    {
        [SerializeField] private CardView _cardPrefab;
        [SerializeField] private Transform _cardContainer;
        [SerializeField] private RectTransform _playingFieldCenterPoint;
        
        private readonly List<CardView> _createdCards = new();

        private Transform _transform;
        private ISubscriber<SetUpPlayingField> _setUpSubscriber;
        private IPublisher<CardClickedEvent> _cardClickedEventPublisher;
        private IDisposable _disposableForSubscriptions;
        
        
        [Inject]
        private void Initialize(ISubscriber<SetUpPlayingField> setUpSubscriber, 
            IPublisher<CardClickedEvent> cardClickedEventPublisher)
        {
            _transform = transform;
            _setUpSubscriber = setUpSubscriber;
            _cardClickedEventPublisher = cardClickedEventPublisher;
            
            DisposableBagBuilder bagBuilder = DisposableBag.CreateBuilder();
            _setUpSubscriber.Subscribe(OnSetUpCommand).AddTo(bagBuilder);
            _disposableForSubscriptions = bagBuilder.Build();
        }

        private void OnSetUpCommand(SetUpPlayingField setUpCommand)
        {
            for (int i = 0; i < setUpCommand.Size.y; i++)
            {
                for (int j = 0; j < setUpCommand.Size.x; j++)
                {
                    CardView newCard = Instantiate(_cardPrefab, _cardContainer);
                    newCard.Initialize($"{nameof(CardView)}_{_createdCards.Count+1}");

                    float xLocalPosition = j == 0
                        ? 0 : 
                        (newCard.HalfWidth * 2 + setUpCommand.SpacingBetweenCards) * (j);
                    float yLocalPosition = i == 0 ? 
                        0 :
                        -((newCard.HalfHeight * 2 + setUpCommand.SpacingBetweenCards) * (i));
                    
                    newCard.SetLocalPosition(new Vector2(xLocalPosition, yLocalPosition));
                    _createdCards.Add(newCard);
                }
            }
            
            CenterPlayingFieldOnDedicatedPoint(setUpCommand.Size.x);
            ScalePlayingFieldForScreenResolution();
        }

        private void CenterPlayingFieldOnDedicatedPoint(int fieldXSize)
        {
            Vector2 cardContainerCenter = FindCardContainerCenterPoint(fieldXSize);
            _cardContainer.SetParent(null);
            _transform.position = new Vector3(_playingFieldCenterPoint.position.x, 
                _playingFieldCenterPoint.position.y, 0f);
            _cardContainer.SetParent(_transform);
            _cardContainer.localPosition = new Vector2(-cardContainerCenter.x, -cardContainerCenter.y);
        }

        private Vector2 FindCardContainerCenterPoint(int fieldXSize)
        {
            Vector2 point1 = _createdCards[0].WorldPosition;
            Vector2 point2 = _createdCards[fieldXSize - 1].WorldPosition;
            Vector2 point3 = _createdCards[^1].WorldPosition;

            float centerXPosition = point1.x + Vector2.Distance(point2, point1) / 2f;
            float centerYPosition = point2.y - Vector2.Distance(point2, point3) / 2f;
            
            return new(centerXPosition, centerYPosition);
        }
        
        private void ScalePlayingFieldForScreenResolution()
        {
            Vector3[] worldCorners = new Vector3[4];
            _playingFieldCenterPoint.GetWorldCorners(worldCorners);
            
            
            /*float xSurpass = Vector2.Distance(_createdCards[0].WorldPosition, worldCorners[1]);
            float ySurpass = _createdCards[0].WorldPosition.y - worldCorners[1].y;*/
            /*float xSurpass = _createdCards[0].WorldPosition.x - worldCorners[1].x;
            float ySurpass = _createdCards[0].WorldPosition.y - worldCorners[1].y;*/
            
            
            float xSurpass = worldCorners[1].x - _createdCards[0].WorldPosition.x;
            float ySurpass = _createdCards[0].WorldPosition.y - worldCorners[1].y;
            float absXSurpass = Mathf.Abs(xSurpass);
            float absYSurpass = Mathf.Abs(ySurpass);
            Debug.Log($"worldCorner: {worldCorners[1]}");
            Debug.Log($"xSurpass: {xSurpass}");
            Debug.Log($"ySurpass: {ySurpass}");
            
            if (xSurpass < 0f && ySurpass < 0f && absXSurpass > 1f && absYSurpass > 1f)
            {
                float scaleFactor = -(ySurpass > xSurpass ? ySurpass : xSurpass);
                _transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            }

            float scaleDownFactor = 0.013f;
            
            if (xSurpass > 0f || ySurpass > 0f)
            {
                float maxSurpass = ySurpass < xSurpass ? xSurpass : ySurpass;
                maxSurpass = 1f - (maxSurpass * 10 * scaleDownFactor);
                _transform.localScale = new Vector3(maxSurpass, maxSurpass, maxSurpass);
            }
        }
    }
}
