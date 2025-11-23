using System;
using System.Collections.Generic;
using MessagePipe;
using TestTankProject.Runtime.MainMenu;
using UnityEngine;
using VContainer;

namespace TestTankProject.Runtime.UI.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private MainMenuButton _mainMenuButtonPrefab;

        private readonly List<MainMenuButton> _createdButtons = new();

        private Transform _transform;
        private IPublisher<MainMenuButtonPressedEvent> _buttonPressedEventPublisher;
        private IDisposable _disposableForSubscriptions;
        
        [Inject]
        private void Initialize(ISubscriber<SetUpMainMenuView> setUpSubscriber, 
            IPublisher<MainMenuButtonPressedEvent> buttonPressedEventPublisher)
        {
            _transform = transform;
            _buttonPressedEventPublisher = buttonPressedEventPublisher;
            
            DisposableBagBuilder bagBuilder = DisposableBag.CreateBuilder();
            setUpSubscriber.Subscribe(OnSetUpCommand).AddTo(bagBuilder);
            _disposableForSubscriptions = bagBuilder.Build();
        }

        private void OnSetUpCommand(SetUpMainMenuView setUpCommand)
        {
            foreach (MainMenuButtonData buttonData in setUpCommand.MainMenuButtons)
            {
                MainMenuButton newButton = Instantiate(_mainMenuButtonPrefab, _transform);
                newButton.Initialize(buttonData.Type, buttonData.Caption);
                newButton.Pressed += OnMainMenuButtonPressed;
                _createdButtons.Add(newButton);
            }
        }

        private void OnMainMenuButtonPressed(MainMenuButtonTypes pressedButtonType)
        {
            _buttonPressedEventPublisher.Publish(new MainMenuButtonPressedEvent(pressedButtonType));
        }

        private void OnDestroy()
        {
            foreach (MainMenuButton createdButton in _createdButtons)
            {
                createdButton.Pressed -= OnMainMenuButtonPressed;
                createdButton.Destroy();
            }
            
            _disposableForSubscriptions?.Dispose();
        }
    }
}
