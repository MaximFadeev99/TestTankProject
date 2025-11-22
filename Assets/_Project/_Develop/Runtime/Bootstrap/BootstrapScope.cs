using System.Collections.Generic;
using MessagePipe;
using TestTankProject.Runtime.Gameplay;
using TestTankProject.Runtime.MainMenu;
using TestTankProject.Runtime.PlayingField;
using TestTankProject.Runtime.SceneLoading;
using TestTankProject.Runtime.UI.MainMenu;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TestTankProject.Runtime.Bootstrap
{
    public class BootstrapScope : LifetimeScope
    {
        [SerializeField] private List<GameConfig> _registeredGameConfigs;
        [SerializeField] private List<CardIconConfig> _registeredCardIconConfigs;
        
        protected override void Configure(IContainerBuilder builder)
        {
            MessagePipeOptions options = builder.RegisterMessagePipe();
            RegisterMessageBrokers(builder, options);
            
            builder.RegisterInstance(Camera.main);
            builder.RegisterInstance(_registeredGameConfigs);
            builder.RegisterInstance(_registeredCardIconConfigs);
            builder.Register<SceneLoader>(Lifetime.Singleton);
                
            builder.RegisterEntryPoint<BootstrapFlow>();
        }
        
        private void RegisterMessageBrokers(IContainerBuilder builder, MessagePipeOptions messagePipeOptions)
        {
            builder.RegisterMessageBroker<SetUpMainMenuView>(messagePipeOptions);
            builder.RegisterMessageBroker<MainMenuButtonPressedEvent>(messagePipeOptions);
            builder.RegisterMessageBroker<CardClickedEvent>(messagePipeOptions);
            builder.RegisterMessageBroker<SetUpPlayingField>(messagePipeOptions);
        }
    }
}