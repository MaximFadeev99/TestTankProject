using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TestTankProject.Runtime.MainMenu
{
    public class MainMenuScope : LifetimeScope
    {
        [SerializeField] private MainMenuConfig _mainMenuConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_mainMenuConfig);
            builder.RegisterEntryPoint<MainMenuFlow>();
        }
    }
}
