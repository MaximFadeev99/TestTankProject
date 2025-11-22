using MessagePipe;
using TestTankProject.Runtime.SceneLoading;
using VContainer;
using VContainer.Unity;

namespace TestTankProject.Runtime.Bootstrap
{
    public class BootstrapScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();
            builder.Register<SceneLoader>(Lifetime.Singleton);
                
            builder.RegisterEntryPoint<BootstrapFlow>();
        }
    }
}