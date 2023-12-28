using Narratore.AI;
using Narratore.Pools;
using Narratore.Solutions.Battle;
using Narratore.UnityUpdate;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Narratore.DI
{
    public class NNYEnemiesConfigurator : Configurator
    {
        [Header("WAVES")]
        [SerializeField] private SpawnWavesConfig[] _waves;

        [Header("SPAWN POINTS")]
        [SerializeField] private RandomOutCameraHeldPointsConfig _spawnPointsConfig;

        [Header("CREEPERS")]
        [SerializeField] private CreeperPoolConfig _creeperPoolConfig;
        [SerializeField] private float _creeperExplosionDistance;


        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            builder.Register<UnitsWavesSpawner>(Lifetime.Singleton).As<IUnitsWavesSpawner>().WithParameter(PlayersIds.GetBotId(1));
            builder.RegisterInstance(_waves).As<IReadOnlyList<SpawnWavesConfig>>();

            RegisterSpawnPoints(builder);
            RegisterEnemiesMove(builder);
            RegisterEntitiesAspects(builder);

            RegisterCreepers(builder, sampleData);
        }


        private void RegisterSpawnPoints(IContainerBuilder builder)
        {
            builder.Register<RandomOutCameraHeldPoints>(Lifetime.Singleton).As<IHeldPoints>().WithParameter(_spawnPointsConfig);
        }

        private void RegisterEnemiesMove(IContainerBuilder builder)
        { 
            builder.Register<SeekSteering>(Lifetime.Singleton).WithParameter(0.01f);
            builder.Register<EnemiesMover>(Lifetime.Singleton).As<ITickable>();
        }

        private void RegisterEntitiesAspects(IContainerBuilder builder)
        {
            builder.Register<EntitiesAspects<MovableBot>>(Lifetime.Singleton).AsSelf().As<IEntitiesAspects<MovableBot>>();
            builder.Register<EntitiesAspects<CreeperDeathExplosion>>(Lifetime.Singleton).AsSelf().As<IEntitiesAspects<CreeperDeathExplosion>>();
        }

        private void RegisterCreepers(IContainerBuilder builder, SampleData sampleData)
        {
            builder.RegisterInstance(new MBPool<CreeperRoster>(_creeperPoolConfig, sampleData)).As<IDisposable>().AsSelf();
            builder.Register<CreeperBattleRegistrator>(Lifetime.Singleton).As<EntityBattleRegistrator<CreeperRoster>>();
            builder.Register<PoolBattleEntitiesSource<CreeperRoster>>(Lifetime.Singleton).As<IBattleEntitiesSource<CreeperRoster>, IDisposable>();
            builder.Register<UnitsSpawner<CreeperRoster>>(Lifetime.Singleton).As<IUnitsSpawner<CreeperRoster>, IUnitsSpawner, IDisposable>().WithParameter(100);

            builder.Register<CreeperSelfExplosionDeathSource>(Lifetime.Singleton).AsSelf().As<ITickable, IDisposable>().WithParameter(_creeperExplosionDistance);
        }
    }
}

