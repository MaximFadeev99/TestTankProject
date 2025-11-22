using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TestTankProject.Runtime.Bootstrap;
using TestTankProject.Runtime.Utilities;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay
{
    public class GameplayManager
    {
        private readonly GameConfig _selectedGameConfig;

        public GameplayManager(IReadOnlyList<GameConfig> allRegisteredGameConfigs)
        {
            try
            {
                _selectedGameConfig = allRegisteredGameConfigs.Single(config => config.StartGameWithMe);
            }
            catch(Exception _)
            {
               CustomLogger.Log($"{nameof(GameplayManager)}", "One and only ONE of Game Configs in " +
                               $"{nameof(BootstrapScope)} prefab must have StartWithMe = true. " +
                               $"Check the prefab and mark ONE of the configs in it!", 
                   MessageTypes.Exception, RecipientTypes.GD ); 
            }
        }
    }
}
