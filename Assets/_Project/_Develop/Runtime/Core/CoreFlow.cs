using System.Collections.Generic;
using MessagePipe;
using TestTankProject.Runtime.Gameplay;
using TestTankProject.Runtime.PlayingField;
using TestTankProject.Runtime.SceneLoading;
using TestTankProject.Runtime.UI.MainMenu;
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
        private readonly IObjectResolver _objectResolver;
        private readonly Raycaster _raycaster;

        public CoreFlow(Camera mainCamera, Canvas sceneCanvas, SceneLoader sceneLoader, Raycaster raycaster,
            IObjectResolver objectResolver)
        {
            _mainCamera = mainCamera;
            _sceneCanvas = sceneCanvas;
            _sceneLoader = sceneLoader;
            _objectResolver = objectResolver;
            _raycaster = raycaster;
        }

        public void Start()
        {
            _sceneLoader.MoveObjectToScene(_mainCamera.gameObject, ProjectScenes.Core);
            _sceneCanvas.worldCamera = _mainCamera;
            _sceneCanvas.planeDistance = 5f;
            
            GameplayManager gameplayManager = new GameplayManager
                (_objectResolver.Resolve<List<GameConfig>>(), 
                    _objectResolver.Resolve<List<CardIconConfig>>(),
                    _objectResolver.Resolve<IPublisher<SetUpPlayingField>>(),
                    _objectResolver.Resolve<IPublisher<UpdateCard>>(),
                    _objectResolver.Resolve<IPublisher<UpdateScoreboard>>(),
                    _objectResolver.Resolve<ISubscriber<CardClickedEvent>>(),
                    _objectResolver.Resolve<ISubscriber<PlayingFieldSetUpEvent>>());
            gameplayManager.StartGame();
        }
    }
}