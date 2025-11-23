using System;
using MessagePipe;
using TestTankProject.Runtime.SceneLoading;
using TestTankProject.Runtime.UI.MainMenu;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TestTankProject.Runtime.MainMenu
{
    public class MainMenuFlow : IStartable
    {
        private readonly Camera _mainCamera;
        private readonly Canvas _sceneCanvas;
        private readonly SceneLoader _sceneLoader;
        private readonly IObjectResolver _objectResolver;

        public MainMenuFlow(Camera mainCamera, Canvas sceneCanvas, SceneLoader sceneLoader,
            IObjectResolver objectResolver)
        {
            _mainCamera = mainCamera;
            _sceneCanvas = sceneCanvas;
            _sceneLoader = sceneLoader;
            _objectResolver = objectResolver;
        }

        public void Start()
        {
            _sceneLoader.MoveObjectToScene(_mainCamera.gameObject, ProjectScenes.MainMenu);
            _sceneCanvas.worldCamera = _mainCamera;
            _sceneCanvas.planeDistance = 5f;
            
            MainMenuManager mainMenuManager = new MainMenuManager(_objectResolver.Resolve<MainMenuConfig>(), 
                _objectResolver.Resolve<IPublisher<SetUpMainMenuView>>(), 
                _objectResolver.Resolve<ISubscriber<MainMenuButtonPressedEvent>>(), 
                _sceneLoader, _mainCamera);
            mainMenuManager.SetUpMainMenu();
        }
    }
}