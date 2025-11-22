using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MessagePipe;
using TestTankProject.Runtime.Bootstrap;
using TestTankProject.Runtime.Gameplay.CardGeneration;
using TestTankProject.Runtime.PlayingField;
using TestTankProject.Runtime.Utilities;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay
{
    public class GameplayManager
    {
        private readonly GameConfig _selectedGameConfig;
        private readonly CardIconConfig _selectedCardIconConfig;
        private readonly IPublisher<SetUpPlayingField> _setUpPlayingFieldPublisher;
        private readonly ICardGenerator _cardGenerator;

        public GameplayManager(IReadOnlyList<GameConfig> allRegisteredGameConfigs, 
            IReadOnlyList<CardIconConfig> allRegisteredCardIconConfigs,
            IPublisher<SetUpPlayingField> setUpPlayingFieldPublisher)
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
            _cardGenerator = _selectedGameConfig.ShallShuffleCards ? null : new OrderedGameGeneration();
        }

        public void StartGame()
        {
            _cardGenerator.GenerateCards(_selectedGameConfig.PlayingFieldSize, _selectedCardIconConfig.CardIcons, 
                out IReadOnlyList<CardDataForView> cardsForView);
            _setUpPlayingFieldPublisher.Publish(new SetUpPlayingField(_selectedGameConfig.PlayingFieldSize, 
                _selectedGameConfig.SpacingBetweenCards, cardsForView));
        }
    }
}
