using System.Collections.Generic;
using MessagePipe;
using TestTankProject.Runtime.Gameplay;
using TestTankProject.Runtime.SceneLoading;
using TestTankProject.Runtime.UI.MainMenu;
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

        public CoreFlow(Camera mainCamera, Canvas sceneCanvas, SceneLoader sceneLoader,
            IObjectResolver objectResolver)
        {
            _mainCamera = mainCamera;
            _sceneCanvas = sceneCanvas;
            _sceneLoader = sceneLoader;
            _objectResolver = objectResolver;
        }

        public void Start()
        {
            _sceneLoader.MoveObjectToScene(_mainCamera.gameObject, ProjectScenes.Core);
            _sceneCanvas.worldCamera = _mainCamera;
            _sceneCanvas.planeDistance = 5f;
            
            GameplayManager gameplayManager = new GameplayManager
                (_objectResolver.Resolve<List<GameConfig>>());
        }
    }
}