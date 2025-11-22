using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MessagePipe;
using TestTankProject.Runtime.Bootstrap;
using TestTankProject.Runtime.PlayingField;
using TestTankProject.Runtime.Utilities;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay
{
    public class GameplayManager
    {
        private readonly GameConfig _selectedGameConfig;
        private readonly IPublisher<SetUpPlayingField> _setUpPlayingFieldPublisher;

        public GameplayManager(IReadOnlyList<GameConfig> allRegisteredGameConfigs, 
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
            
            _setUpPlayingFieldPublisher = setUpPlayingFieldPublisher;
        }

        public void StartGame()
        {
            _setUpPlayingFieldPublisher.Publish(new SetUpPlayingField(_selectedGameConfig.PlayingFieldSize, 
                _selectedGameConfig.SpacingBetweenCards));
        }
    }
}
