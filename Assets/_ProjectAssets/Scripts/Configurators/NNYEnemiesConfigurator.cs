using Narratore.Pools;
using Narratore.Solutions.Battle;
using Narratore.UnityUpdate;
using System;
using UnityEngine;
using VContainer;

namespace Narratore.DI
{
    public class NNYEnemiesConfigurator : Configurator
    {
        [SerializeField] private HeldPoints _heldPoints;

        [Header("UNITS POOLS")]
        [SerializeField] private CreeperPoolConfig _creeperPoolConfig;


        public override void Configure(IContainerBuilder builder, LevelConfig config, Updatables prepared, Updatables beginned)
        {
            builder.RegisterInstance(_heldPoints).As<IHeldPoints>();

            RegisterCreepers(builder);
        }

        private void RegisterCreepers(IContainerBuilder builder)
        {
            int count = _creeperPoolConfig.StartItemsCount;

            builder.RegisterInstance(new MBPool<CreeperRoster>(_creeperPoolConfig)).As<IDisposable>().AsSelf();
            builder.Register<CreeperBattleRegistrator>(Lifetime.Singleton).As<EntityBattleRegistrator<CreeperRoster>>();
            builder.Register<PoolBattleEntitiesSource<CreeperRoster>>(Lifetime.Singleton).As<IBattleEntitiesSource<CreeperRoster>, IDisposable>();
            builder.Register<UnitsSpawner<CreeperRoster>>(Lifetime.Singleton).As<IUnitsSpawner<CreeperRoster>, IDisposable>().WithParameter(count);
        }
    }
}

