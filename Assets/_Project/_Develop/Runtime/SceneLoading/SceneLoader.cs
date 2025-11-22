using System;
using TestTankProject.Runtime.Utilities;
using UnityEngine.SceneManagement;

namespace TestTankProject.Runtime.SceneLoading
{
    public class SceneLoader
    {
        private const string BootstrapSceneName = "1.Bootstrap (START_HERE)";
        private const string MainMenuSceneName = "2.MainMenu";
        private const string CoreSceneName = "3.Core";

        public void LoadScene(ProjectScenes targetScene)
        {
            string targetSceneName = GetSceneNameByEnum(targetScene);
            SceneManager.LoadScene(targetSceneName);
        }

        private string GetSceneNameByEnum(ProjectScenes targetScene)
        {
            return targetScene switch
            {
                ProjectScenes.Bootstrap => BootstrapSceneName,
                ProjectScenes.MainMenu => MainMenuSceneName,
                ProjectScenes.Core => CoreSceneName,
                _ => throw new NotImplementedException($"[{nameof(SceneLoader)}]: FAILED to load scene {targetScene}, " +
                                                       "since no name for it is registered!")
            };
        }
    }
}
