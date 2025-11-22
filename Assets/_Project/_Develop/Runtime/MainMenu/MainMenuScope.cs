using VContainer;
using VContainer.Unity;

namespace TestTankProject.Runtime.MainMenu
{
    public class MainMenuScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MainMenuFlow>();
        }
    }
}
