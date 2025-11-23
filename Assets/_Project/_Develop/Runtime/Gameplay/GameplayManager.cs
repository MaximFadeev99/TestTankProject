using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BaseBuilding.Tests;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TestTankProject.Runtime.Bootstrap;
using TestTankProject.Runtime.Gameplay.GameGeneration;
using TestTankProject.Runtime.PlayingField;
using TestTankProject.Runtime.UI.Scoreboard;
using TestTankProject.Runtime.Utilities;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay
{
    public class GameplayManager
    {
        private readonly GameConfig _selectedGameConfig;
        private readonly CardIconConfig _selectedCardIconConfig;
        public readonly SpriteLoader _spriteLoader;
        private readonly IGameGenerator _iGameGenerator;
        
        private readonly IPublisher<SetUpPlayingField> _setUpPlayingFieldPublisher;
        private readonly IPublisher<UpdateCard> _updateCardPublisher;
        private readonly IPublisher<UpdateScoreboard> _updateScoreboardPublisher;
        private readonly IDisposable _disposableForSubscriptions;

        private CancellationTokenSource _gameQuitCts;
        private GameModel _currentGame;

        public GameplayManager(IReadOnlyList<GameConfig> allRegisteredGameConfigs, 
            IReadOnlyList<CardIconConfig> allRegisteredCardIconConfigs,
            SpriteLoader spriteLoader,
            IPublisher<SetUpPlayingField> setUpPlayingFieldPublisher,
            IPublisher<UpdateCard> updateCardPublisher,
            IPublisher<UpdateScoreboard> updateScoreboardPublisher,
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
            
            _spriteLoader = spriteLoader;
            _setUpPlayingFieldPublisher = setUpPlayingFieldPublisher;
            _updateCardPublisher = updateCardPublisher;
            _updateScoreboardPublisher = updateScoreboardPublisher;
            _iGameGenerator = _selectedGameConfig.ShallShuffleCards ? 
                new RandomGameGenerator() : new OrderedGameGeneration();
            
            DisposableBagBuilder bagBuilder = DisposableBag.CreateBuilder();
            cardClickedSubscriber.Subscribe(OnCardClickedEvent).AddTo(bagBuilder);
            playingFieldSetUpSubscriber.Subscribe(OnPlayingFieldSetUpEvent).AddTo(bagBuilder);
            _disposableForSubscriptions = bagBuilder.Build();
        }

        public async void StartGame()
        {
            GameGenerationResult gameGenerationResult = await _iGameGenerator
                .GenerateGame(_selectedGameConfig.PlayingFieldSize, _spriteLoader, _selectedCardIconConfig.IconReferences);
            _currentGame = new GameModel(_selectedGameConfig.InitialCardDemonstrationTime, 
                _selectedGameConfig.CardDisappearDelay, _selectedGameConfig.PointsPerMatch, _selectedGameConfig.PointsPerMatchStreak,
                gameGenerationResult.Cards, 0, 0,0,0);
            _gameQuitCts = new CancellationTokenSource();
            
            _currentGame.ShowCard += OnShowCardCommand;
            _currentGame.TurnCompleted += OnTurnCompleted;
            _currentGame.CardsMatched += OnCardsMatched;
            _currentGame.CardsMismatched += OnCardsMismatched;
            _currentGame.GameCompleted += OnGameCompleted;
            
            _setUpPlayingFieldPublisher.Publish(new SetUpPlayingField(_selectedGameConfig.PlayingFieldSize, 
                _selectedGameConfig.SpacingBetweenCards, gameGenerationResult.CardsForView));
            _updateScoreboardPublisher.Publish(new UpdateScoreboard(_currentGame.CurrentPoints, 
                _currentGame.BonusPoints, _currentGame.CurrentMatches, _currentGame.TotalMatchAttempts));
        }

        public void SaveGame()
        {
            Dispose();
        }

        private async void OnPlayingFieldSetUpEvent(PlayingFieldSetUpEvent _)
        {
            if (_currentGame == null)
                return;
            
            foreach (CardModel card in _currentGame.Cards)
            {
                _updateCardPublisher.Publish(new UpdateCard(card.Address, CardActions.RaiseCover));
                await UniTask.WaitForSeconds(0.2f, cancellationToken: _gameQuitCts.Token)
                    .SuppressCancellationThrow();

                if (_gameQuitCts.IsCancellationRequested)
                    return;
            }
            
            await UniTask.WaitForSeconds(_selectedGameConfig.InitialCardDemonstrationTime
                , cancellationToken: _gameQuitCts.Token).SuppressCancellationThrow();;
            
            if (_gameQuitCts.IsCancellationRequested || _currentGame == null)
                return;
            
            foreach (CardModel card in _currentGame.Cards)
            {
                _updateCardPublisher.Publish(new UpdateCard(card.Address, CardActions.PutDownCover));
                await UniTask.WaitForSeconds(0.2f, cancellationToken: _gameQuitCts.Token)
                    .SuppressCancellationThrow();;
                
                if (_gameQuitCts.IsCancellationRequested)
                    return;
            }

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
            _updateScoreboardPublisher.Publish(new UpdateScoreboard(_currentGame.CurrentPoints, 
                _currentGame.BonusPoints, _currentGame.CurrentMatches, _currentGame.TotalMatchAttempts));
        }

        private void OnGameCompleted()
        {
            Dispose();
            Debug.Log("Game Completed!");
        }

        private void Dispose()
        {
            _disposableForSubscriptions?.Dispose();
            _gameQuitCts?.Cancel();
            _currentGame.ShowCard -= OnShowCardCommand;
            _currentGame.TurnCompleted -= OnTurnCompleted;
            _currentGame.CardsMatched -= OnCardsMatched;
            _currentGame.CardsMismatched -= OnCardsMismatched;
            _currentGame.GameCompleted -= OnGameCompleted;
            _currentGame = null;
        }
    }
}
