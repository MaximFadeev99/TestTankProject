using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TestTankProject.Runtime.Bootstrap;
using TestTankProject.Runtime.Gameplay.GameGeneration;
using TestTankProject.Runtime.PlayingField;
using TestTankProject.Runtime.Utilities;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay
{
    public class GameplayManager
    {
        private readonly GameConfig _selectedGameConfig;
        private readonly CardIconConfig _selectedCardIconConfig;
        private readonly IGameGenerator _iGameGenerator;
        
        private readonly IPublisher<SetUpPlayingField> _setUpPlayingFieldPublisher;
        private readonly IPublisher<UpdateCard> _updateCardPublisher;
        private IDisposable _disposableForSubscriptions;
        
        private GameModel _currentGame;

        public GameplayManager(IReadOnlyList<GameConfig> allRegisteredGameConfigs, 
            IReadOnlyList<CardIconConfig> allRegisteredCardIconConfigs,
            IPublisher<SetUpPlayingField> setUpPlayingFieldPublisher,
            IPublisher<UpdateCard> updateCardPublisher,
            ISubscriber<CardClickedEvent> cardClickedSubscriber,
            ISubscriber<PlayingFieldSetUpEvent> playingFieldSetUpSubscriber)
        {
            try
            {
                _selectedGameConfig = allRegisteredGameConfigs.Single(config => config.StartGameWithMe);
            }
            catch
            {
               CustomLogger.Log($"{nameof(GameplayManager)}", "One and only ONE of Game Configs in " +
                               $"{nameof(BootstrapScope)} prefab must have StartWithMe = true. " +
                               $"Check the prefab and mark ONE of the configs in it!", 
                   MessageTypes.Exception, RecipientTypes.GD ); 
            }
            
            try
            {
                _selectedCardIconConfig = allRegisteredCardIconConfigs.Single(config => config.StartWithMe);
            }
            catch
            {
                CustomLogger.Log($"{nameof(GameplayManager)}", "One and only ONE of Card Icon Configs in " +
                                                               $"{nameof(BootstrapScope)} prefab must have StartWithMe = true. " +
                                                               $"Check the prefab and mark ONE of the configs in it!", 
                    MessageTypes.Exception, RecipientTypes.GD ); 
            }
            
            _setUpPlayingFieldPublisher = setUpPlayingFieldPublisher;
            _updateCardPublisher = updateCardPublisher;
            _iGameGenerator = _selectedGameConfig.ShallShuffleCards ? null : new OrderedGameGeneration();
            
            DisposableBagBuilder bagBuilder = DisposableBag.CreateBuilder();
            cardClickedSubscriber.Subscribe(OnCardClickedEvent).AddTo(bagBuilder);
            playingFieldSetUpSubscriber.Subscribe(OnPlayingFieldSetUpEvent).AddTo(bagBuilder);
            _disposableForSubscriptions = bagBuilder.Build();
        }

        public void StartGame()
        {
            IReadOnlyList<CardModel> cards = _iGameGenerator.GenerateGame(_selectedGameConfig.PlayingFieldSize, 
                _selectedCardIconConfig.CardIcons, out IReadOnlyList<CardDataForView> cardsForView);
            
            _currentGame = new GameModel(_selectedGameConfig.InitialCardDemonstrationTime, 
                _selectedGameConfig.CardDisappearDelay, 
                cards);
            _currentGame.ShowCard += OnShowCardCommand;
            _currentGame.TurnCompleted += OnTurnCompleted;
            _currentGame.CardsMatched += OnCardsMatched;
            _currentGame.CardsMismatched += OnCardsMismatched;
            _currentGame.GameCompleted += OnGameCompleted;
            _setUpPlayingFieldPublisher.Publish(new SetUpPlayingField(_selectedGameConfig.PlayingFieldSize, 
                _selectedGameConfig.SpacingBetweenCards, cardsForView));
        }

        private void OnPlayingFieldSetUpEvent(PlayingFieldSetUpEvent _)
        {
            _currentGame.Status = GameStatus.Running;
        }

        private void OnCardClickedEvent(CardClickedEvent cardClickedEvent)
        {
            if (_currentGame == null || _currentGame.Status != GameStatus.Running)
                return;

            _currentGame.SelectCard(cardClickedEvent.CardAddress);
        }

        private async void OnCardsMatched(Vector2Int cardAddress1, Vector2Int cardAddress2)
        {
            await UniTask.WaitForSeconds(_currentGame.CardDisappearDelay);
            
            _updateCardPublisher.Publish(new(cardAddress1, CardActions.Remove));
            _updateCardPublisher.Publish(new(cardAddress2, CardActions.Remove));
        }

        private async void OnCardsMismatched(Vector2Int cardAddress1, Vector2Int cardAddress2)
        {
            await UniTask.WaitForSeconds(_currentGame.CardDisappearDelay);
            
            _updateCardPublisher.Publish(new(cardAddress1, CardActions.PutDownCover));
            _updateCardPublisher.Publish(new(cardAddress2, CardActions.PutDownCover));
        }

        private void OnShowCardCommand(Vector2Int cardAddress)
        {
            _updateCardPublisher.Publish(new(cardAddress, CardActions.RaiseCover));
        }

        private void OnTurnCompleted()
        {
            
        }

        private void OnGameCompleted()
        {
            _disposableForSubscriptions?.Dispose();
            _currentGame.ShowCard -= OnShowCardCommand;
            _currentGame.TurnCompleted -= OnTurnCompleted;
            _currentGame.CardsMatched -= OnCardsMatched;
            _currentGame.CardsMismatched -= OnCardsMismatched;
            _currentGame.GameCompleted -= OnGameCompleted;
            _currentGame = null;
            Debug.Log("Game Completed!");
        }
    }
}
