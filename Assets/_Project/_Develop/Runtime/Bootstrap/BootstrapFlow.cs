using TestTankProject.Runtime.SceneLoading;
using UnityEngine;
using VContainer.Unity;

namespace TestTankProject.Runtime
{
    public class BootstrapFlow : IStartable
    {
        private readonly SceneLoader _sceneLoader;

        public BootstrapFlow(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Start()
        {
            SetApplicationSettings();
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