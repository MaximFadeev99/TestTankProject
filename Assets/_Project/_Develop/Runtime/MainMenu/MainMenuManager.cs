using System;
using MessagePipe;
using TestTankProject.Runtime.SceneLoading;
using TestTankProject.Runtime.UI.MainMenu;
using TestTankProject.Runtime.Utilities;
using UnityEngine;

namespace TestTankProject.Runtime.MainMenu
{
    public class MainMenuManager
    {
        private readonly Camera _mainCamera;
        private readonly MainMenuConfig _mainMenuConfig;
        private readonly SceneLoader _sceneLoader;
        private readonly IPublisher<SetUpMainMenuView> _setUpCommandPublisher;
        private readonly ISubscriber<MainMenuButtonPressedEvent> _mainMenuButtonPressedSubscriber;
        
        private IDisposable _disposableForSubscriptions;
        
        
        public MainMenuManager(MainMenuConfig mainMenuConfig, IPublisher<SetUpMainMenuView> setUpCommandPublisher,
            ISubscriber<MainMenuButtonPressedEvent> mainMenuButtonPressedSubscriber, SceneLoader sceneLoader, 
            Camera mainCamera)
        {
            _mainMenuConfig = mainMenuConfig;
            _setUpCommandPublisher = setUpCommandPublisher;
            _mainMenuButtonPressedSubscriber = mainMenuButtonPressedSubscriber;
            _sceneLoader = sceneLoader;
            _mainCamera = mainCamera;
        }

        public void SetUpMainMenu()
        {
            DisposableBagBuilder bagBuilder = DisposableBag.CreateBuilder();
            _mainMenuButtonPressedSubscriber.Subscribe(OnMainMenuButtonPressedEvent).AddTo(bagBuilder);
            _disposableForSubscriptions = bagBuilder.Build();
            
            _setUpCommandPublisher.Publish(new SetUpMainMenuView(_mainMenuConfig.MainMenuButtons));
        }

        private void OnMainMenuButtonPressedEvent(MainMenuButtonPressedEvent pressedEvent)
        {
            switch (pressedEvent.PressedButtonType)
            {
                case MainMenuButtonTypes.Play:
                    GameObject.DontDestroyOnLoad(_mainCamera);
                    _sceneLoader.LoadScene(ProjectScenes.Core);
                    break;
                
                case MainMenuButtonTypes.Quit:
                    Application.Quit();
                    break;
                
                default:
                    CustomLogger.Log($"{nameof(MainMenuManager)}", 
                        $"No action implemented for button with type {pressedEvent.PressedButtonType}!", 
                        MessageTypes.Error);
                    break;
            }
        }
    }
}