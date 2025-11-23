using MessagePipe;
using TestTankProject.Runtime.UserInput;
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
        
        private void OnDestroy()
        {
            Container.Dispose();
        }
    }
}
