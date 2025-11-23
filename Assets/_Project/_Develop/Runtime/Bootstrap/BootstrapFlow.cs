using System;
using TestTankProject.Runtime.SceneLoading;
using TestTankProject.Runtime.Sounds;
using TestTankProject.Runtime.UserInput;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TestTankProject.Runtime
{
    public class BootstrapFlow : IStartable, IDisposable
    {
        private readonly SceneLoader _sceneLoader;
        private readonly Camera _mainCamera;
        private readonly InputLogger _inputLogger;
        private readonly IObjectResolver _objectResolver;

        public BootstrapFlow(SceneLoader sceneLoader, Camera mainCamera, InputLogger inputLogger,
             IObjectResolver objectResolver)
        {
            _sceneLoader = sceneLoader;
            _mainCamera = mainCamera;
            _inputLogger = inputLogger;
            _objectResolver = objectResolver;
        }

        public void Start()
        {
            SetApplicationSettings();
            
            AudioManager audioManager = GameObject.FindObjectOfType<AudioManager>();
            _objectResolver.Inject(audioManager);
            
            GameObject.DontDestroyOnLoad(_mainCamera);
            GameObject.DontDestroyOnLoad(audioManager);
            
            _sceneLoader.LoadScene(ProjectScenes.MainMenu);
        }

        private void SetApplicationSettings()
        {
            Application.runInBackground = false;
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = false;
        }

        public void Dispose()
        {
            
        }
    }
}