using System;
using System.Collections.Generic;
using BaseBuilding.Tests;
using MessagePipe;
using TestTankProject.Runtime.Core.Sounds;
using TestTankProject.Runtime.Gameplay;
using TestTankProject.Runtime.PlayingField;
using TestTankProject.Runtime.SaveLoad;
using TestTankProject.Runtime.SceneLoading;
using TestTankProject.Runtime.UI.EndGamePanel;
using TestTankProject.Runtime.UI.EndGamePanel.Commands;
using TestTankProject.Runtime.UI.Scoreboard;
using TestTankProject.Runtime.UserInput;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TestTankProject.Runtime.Core
{
    public class CoreFlow : IStartable
    {
        private readonly Camera _mainCamera;
        private readonly Canvas _sceneCanvas;
        private readonly SceneLoader _sceneLoader;
        private readonly SpriteLoader _spriteLoader;
        private readonly IObjectResolver _objectResolver;
        private readonly Raycaster _raycaster;
        private readonly IDisposable _disposableForSubscriptions;
        
        private GameplayManager _gameplayManager;

        public CoreFlow(Camera mainCamera, Canvas sceneCanvas, SceneLoader sceneLoader, Raycaster raycaster,
            SpriteLoader spriteLoader, IObjectResolver objectResolver, 
            ISubscriber<ReturnButtonPressedEvent> returnPressedSubscriber)
        {
            _mainCamera = mainCamera;
            _sceneCanvas = sceneCanvas;
            _sceneLoader = sceneLoader;
            _objectResolver = objectResolver;
            _raycaster = raycaster;
            _spriteLoader = spriteLoader;
            
            DisposableBagBuilder bagBuilder = DisposableBag.CreateBuilder();
            returnPressedSubscriber.Subscribe(OnReturnPressed).AddTo(bagBuilder);
            _disposableForSubscriptions = bagBuilder.Build();
        }

        public void Start()
        {
            _sceneLoader.MoveObjectToScene(_mainCamera.gameObject, ProjectScenes.Core);
            _sceneCanvas.worldCamera = _mainCamera;
            _sceneCanvas.planeDistance = 5f;
            
            _gameplayManager = new GameplayManager
                (_objectResolver.Resolve<List<GameConfig>>(), 
                    _objectResolver.Resolve<List<CardIconConfig>>(),
                    _spriteLoader,
                    _objectResolver.Resolve<IPublisher<SetUpPlayingField>>(),
                    _objectResolver.Resolve<IPublisher<UpdateCard>>(),
                    _objectResolver.Resolve<IPublisher<UpdateScoreboard>>(),
                    _objectResolver.Resolve<IPublisher<DrawEndGamePanel>>(),
                    _objectResolver.Resolve<IPublisher<PlaySoundCommand>>(),
                    _objectResolver.Resolve<ISubscriber<CardClickedEvent>>(),
                    _objectResolver.Resolve<ISubscriber<PlayingFieldSetUpEvent>>(),
                    _objectResolver.Resolve<IGameSaver>(),
                    _objectResolver.Resolve<IGameLoader>());
            _gameplayManager.StartGame();
        }

        public void Dispose()
        {
            _disposableForSubscriptions?.Dispose();
            _spriteLoader.ReleaseAllDistributedSprites();
            
            if (_gameplayManager != null)
                _gameplayManager.SaveGame();
            
            _gameplayManager = null;
            GameObject.DontDestroyOnLoad(_mainCamera);
            _sceneLoader.LoadScene(ProjectScenes.MainMenu);
        }

        private void OnReturnPressed(ReturnButtonPressedEvent _)
        {
            Dispose();
        }
    }
}