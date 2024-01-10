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

        [Header("ENEMIES")]
        [SerializeField] private BaseCreeperPoolConfig _baseConfig;
        [SerializeField] private FastCreeperPoolConfig _fastConfig;
        [SerializeField] private BossCreeperPoolConfig _bossConfig;
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

            RegisterBaseCreepers(builder, sampleData);
            RegisterFastCreepers(builder, sampleData);
            RegisterBossCreepers(builder, sampleData);

            builder.Register<CreeperSelfExplosionDeathSource>(Lifetime.Singleton)
                .AsSelf().As<IBeginnedUpdatable>()
                .WithParameter(_creeperExplosionDistance);

            builder.Register<BotsShooting>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(PlayersIds.GetBotId(1));
        }


        private void RegisterSpawnPoints(IContainerBuilder builder)
        {
            builder.Register<PlayerDirectionMoveHeldPoints>(Lifetime.Singleton).As<IHeldPoints>().WithParameter(_spawnPointsConfig);
        }

        private void RegisterEnemiesMove(IContainerBuilder builder)
        { 
            builder.Register<SeekSteering>(Lifetime.Singleton).WithParameter(0.0001f);
            builder.Register<EnemiesMover>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterEntitiesAspects(IContainerBuilder builder)
        {
            builder.Register<EntitiesAspects<MovableBot>>(Lifetime.Singleton).AsSelf().As<IEntitiesAspects<MovableBot>>();
            builder.Register<EntitiesAspects<CreeperDeathExplosion>>(Lifetime.Singleton).AsSelf().As<IEntitiesAspects<CreeperDeathExplosion>>();
            builder.Register<EntitiesAspects<BotShootingConfig>>(Lifetime.Singleton).AsSelf().As<IEntitiesAspects<BotShootingConfig>>();
        }

        private void RegisterBaseCreepers(IContainerBuilder builder, SampleData sampleData)
        {
            builder.RegisterInstance(new MBPool<BaseCreeperRoster>(_baseConfig, sampleData)).As<IDisposable>().AsSelf();
            builder.Register<BaseCreeperBattleRegistrator>(Lifetime.Singleton).As<EntityBattleRegistrator<BaseCreeperRoster>>();
            builder.Register<PoolBattleEntitiesSource<BaseCreeperRoster>>(Lifetime.Singleton).As<IBattleEntitiesSource<BaseCreeperRoster>, IDisposable>();
            builder.Register<UnitsSpawner<BaseCreeperRoster>>(Lifetime.Singleton).As<IUnitsSpawner<BaseCreeperRoster>, IUnitsSpawner, IDisposable>().WithParameter(100);
        }

        private void RegisterFastCreepers(IContainerBuilder builder, SampleData sampleData)
        {
            builder.RegisterInstance(new MBPool<FastCreeperRoster>(_fastConfig, sampleData)).As<IDisposable>().AsSelf();
            builder.Register<FastCreeperBattleRegistrator>(Lifetime.Singleton).As<EntityBattleRegistrator<FastCreeperRoster>>();
            builder.Register<PoolBattleEntitiesSource<FastCreeperRoster>>(Lifetime.Singleton).As<IBattleEntitiesSource<FastCreeperRoster>, IDisposable>();
            builder.Register<UnitsSpawner<FastCreeperRoster>>(Lifetime.Singleton).As<IUnitsSpawner<FastCreeperRoster>, IUnitsSpawner, IDisposable>().WithParameter(100);
        }

        private void RegisterBossCreepers(IContainerBuilder builder, SampleData sampleData)
        {
            builder.RegisterInstance(new MBPool<BossCreeperRoster>(_bossConfig, sampleData)).As<IDisposable>().AsSelf();
            builder.Register<BossCreeperBattleRegistrator>(Lifetime.Singleton).As<EntityBattleRegistrator<BossCreeperRoster>>();
            builder.Register<PoolBattleEntitiesSource<BossCreeperRoster>>(Lifetime.Singleton).As<IBattleEntitiesSource<BossCreeperRoster>, IDisposable>();
            builder.Register<UnitsSpawner<BossCreeperRoster>>(Lifetime.Singleton).As<IUnitsSpawner<BossCreeperRoster>, IUnitsSpawner, IDisposable>().WithParameter(100);
        }
    }
}

