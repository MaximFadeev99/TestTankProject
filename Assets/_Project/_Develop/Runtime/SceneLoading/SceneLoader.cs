using System;
using TestTankProject.Runtime.Utilities;
using UnityEngine;
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
            SceneManager.LoadScene(targetSceneName, LoadSceneMode.Single);
        }

        public void MoveObjectToScene(GameObject gameObject, ProjectScenes targetScene)
        {
            string targetSceneName = GetSceneNameByEnum(targetScene);
            Scene scene = SceneManager.GetSceneByName(targetSceneName);
            SceneManager.MoveGameObjectToScene(gameObject, scene);
        }

        private string GetSceneNameByEnum(ProjectScenes targetScene)
        {
            return targetScene switch
            {
                ProjectScenes.Bootstrap => BootstrapSceneName,
                ProjectScenes.MainMenu => MainMenuSceneName,
                ProjectScenes.Core => CoreSceneName,
                _ => throw new NotImplementedException($"[{nameof(SceneLoader)}]: FAILED to get a scene name for {targetScene}, " +
                                                       "since no name for it is registered!")
            };
        }
    }
}
