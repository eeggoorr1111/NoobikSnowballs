using Narratore.Solutions.Battle;
using Narratore.UI;
using Narratore.UnityUpdate;
using Narratore.WorkWithMesh;
using TMPro;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Narratore.DI
{
    public class NNYLevelAreaConfigurator : Configurator
    {
        [SerializeField] private MeshFrame _area;
        [SerializeField] private EntityCount _outEnemies;
        [SerializeField] private LoopedTextFadeAnimation _warning;
        [SerializeField] private float _spawnDelay;

        public override void Configure(IContainerBuilder builder, LevelConfig config, Updatables preparedUpdatables, Updatables beginnedUpdatables)
        {
            builder.RegisterEntryPoint<LevelAreaHandler>(Lifetime.Singleton).As<ITickable>()
                .WithParameter(_area)
                .WithParameter(_outEnemies)
                .WithParameter(_warning)
                .WithParameter(_spawnDelay);
        }
    }
}

