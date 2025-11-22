using TestTankProject.Runtime.SceneLoading;
using TestTankProject.Runtime.UserInput;
using UnityEngine;
using VContainer.Unity;

namespace TestTankProject.Runtime
{
    public class BootstrapFlow : IStartable
    {
        private readonly SceneLoader _sceneLoader;
        private readonly Camera _mainCamera;
        private readonly InputLogger _inputLogger;

        public BootstrapFlow(SceneLoader sceneLoader, Camera mainCamera, InputLogger inputLogger)
        {
            _sceneLoader = sceneLoader;
            _mainCamera = mainCamera;
            _inputLogger = inputLogger;
        }

        public void Start()
        {
            SetApplicationSettings();
            GameObject.DontDestroyOnLoad(_mainCamera);
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
    }
}