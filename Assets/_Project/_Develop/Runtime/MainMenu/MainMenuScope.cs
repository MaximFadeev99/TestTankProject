using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TestTankProject.Runtime.MainMenu
{
    public class MainMenuScope : LifetimeScope
    {
        [SerializeField] private MainMenuConfig _mainMenuConfig;
        [SerializeField] private Canvas _sceneCanvas;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_mainMenuConfig);
            builder.RegisterInstance(_sceneCanvas);
            builder.RegisterEntryPoint<MainMenuFlow>();
        }
    }
}
