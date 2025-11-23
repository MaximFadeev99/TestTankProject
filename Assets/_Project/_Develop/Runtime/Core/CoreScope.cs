using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TestTankProject.Runtime.Core
{
    public class CoreScope : LifetimeScope
    {
        [SerializeField] private Canvas _sceneCanvas;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_sceneCanvas);
            builder.RegisterEntryPoint<CoreFlow>(Lifetime.Scoped);
        }

        protected override void OnDestroy()
        {
            Container.Dispose();
        }
    }
}
