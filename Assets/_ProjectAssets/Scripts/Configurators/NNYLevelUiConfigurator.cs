using Narratore.Pools;
using Narratore.UI;
using UnityEngine;
using VContainer;

namespace Narratore.DI
{
    public class NNYLevelUiConfigurator : Configurator
    {
        [SerializeField] private ShopWindow _shopWindow;


        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            builder.RegisterInstance(_shopWindow);
        }
    }
}

