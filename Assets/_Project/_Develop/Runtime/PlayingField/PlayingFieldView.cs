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
        [SerializeField] private Transform _playingFieldCenterPoint;
        
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
            float yOffset = 0f;
            float xOffset = 0f;
            
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
        }
    }
}
