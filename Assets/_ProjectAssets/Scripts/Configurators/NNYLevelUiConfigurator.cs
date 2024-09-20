using Narratore.Pools;
using UnityEngine;
using VContainer;

namespace Narratore.DI
{
    public class NNYLevelUiConfigurator : LevelConfigurator
    {
        [SerializeField] private InfoCanvas _infoCanvas;


        protected override void ConfigureImpl(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            builder.Register<InfoCanvasHandler>(Lifetime.Singleton).As<IBegunGameHandler>()
                .WithParameter(_infoCanvas);
        }
    }
}

