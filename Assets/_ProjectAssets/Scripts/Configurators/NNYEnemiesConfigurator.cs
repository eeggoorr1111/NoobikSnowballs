using Narratore.AI;
using Narratore.Pools;
using Narratore.Solutions.Battle;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Narratore.DI
{
    public class NNYEnemiesConfigurator : Configurator
    {
        [Header("VIEW")]
        [SerializeField] private TMP_Text _enemiesCount;

        [Header("SPAWN POINTS")]
        [SerializeField] private RandomOutCameraHeldPointsConfig _spawnPointsConfig;

        [Header("CREEPERS")]
        [SerializeField] private CreeperPoolConfig _creeperPoolConfig;
        [SerializeField] private float _creeperExplosionDistance;


        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            builder.RegisterEntryPoint<SpawnedUnitsCounter>(Lifetime.Singleton).WithParameter(_enemiesCount);
            builder.Register<UnitsWavesSpawner>(Lifetime.Singleton).As<IUnitsWavesSpawner>().WithParameter(PlayersIds.GetBotId(1));

            LevelSpawnWavesConfig[] levels = GetComponentsInChildren<LevelSpawnWavesConfig>();
            LoopedCounter counter = new LoopedCounter(0, levels.Length - 1, config.Level - 1);
            builder.RegisterInstance(levels[counter.Current].Waves).As<IReadOnlyList<SpawnWavesConfig>>();

            RegisterSpawnPoints(builder);
            RegisterEnemiesMove(builder);
            RegisterEntitiesAspects(builder);

            RegisterCreepers(builder, sampleData);
        }


        private void RegisterSpawnPoints(IContainerBuilder builder)
        {
            builder.Register<PlayerDirectionMoveHeldPoints>(Lifetime.Singleton).As<IHeldPoints>().WithParameter(_spawnPointsConfig);
        }

        private void RegisterEnemiesMove(IContainerBuilder builder)
        { 
            builder.Register<SeekSteering>(Lifetime.Singleton).WithParameter(0.01f);
            builder.Register<EnemiesMover>(Lifetime.Singleton).AsImplementedInterfaces();
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

            builder.Register<CreeperSelfExplosionDeathSource>(Lifetime.Singleton)
                .AsSelf().As<IBeginnedUpdatable>()
                .WithParameter(_creeperExplosionDistance);
        }
    }
}

