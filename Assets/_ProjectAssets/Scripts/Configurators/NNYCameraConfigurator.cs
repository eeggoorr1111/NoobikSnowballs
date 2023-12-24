using Narratore.CameraTools;
using Narratore.UnityUpdate;
using VContainer;

namespace Narratore.DI
{
    public class NNYCameraConfigurator : Configurator
    {
        public override void Configure(IContainerBuilder builder, LevelConfig config, Updatables preparedUpdatables, Updatables beginnedUpdatables)
        {
            builder.Register<AlwaysMainCamera>(Lifetime.Singleton).As<ICurrentCameraGetter>();
        }
    }
}

