using Narratore;
using Narratore.DI;
using Narratore.Pools;
using Narratore.Solutions.Battle;
using Narratore.UI;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class NNYLootConfigurator : LootConfigurator<NNYLootDroping, LootDeathSources>
{
    [Header("HEAL")]
    [SerializeField] private HealLootPoolConfig _healPoolConfig;

    [Header("CURRENCY")]
    [SerializeField] private CurrencyLootPoolConfig _currencyPoolConfig;
    [SerializeField] private UiCoinsFlyerPoolConfig _uiCoinsFlyerConfig;
    


    protected override void RegisterEntitiesAspectsImpl(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
    {
        builder.Register<EntitiesAspects<HealLootData>>(Lifetime.Singleton).AsSelf().As<IEntitiesAspects<HealLootData>>();
        builder.Register<EntitiesAspects<CurrencyLootData>>(Lifetime.Singleton).AsSelf().As<IEntitiesAspects<CurrencyLootData>>();
    }

    protected override void RegisterLootCollecting(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
    {
        builder.RegisterEntryPoint<HealLootCollecting>(Lifetime.Singleton);
        builder.RegisterEntryPoint<CurrencyLootCollecting>(Lifetime.Singleton);
    }

    protected override void RegisterSources(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
    {
        builder.RegisterInstance(new MBPools<HealLootRoster, HealLootRoster>(_healPoolConfig, sampleData)).As<IDisposable>().AsSelf();
        builder.RegisterInstance(new MBPools<CurrencyLootRoster, CurrencyLootRoster>(_currencyPoolConfig, sampleData)).As<IDisposable>().AsSelf();
        builder.RegisterInstance(new MBPool<UICoinsFlyer>(_uiCoinsFlyerConfig, sampleData)).As<IMBPool<UICoinsFlyer>, IDisposable>();

        builder.Register<LootSource<HealLootRoster>>(Lifetime.Singleton).As<ILootSource, IDisposable>();
        builder.Register<LootSource<CurrencyLootRoster>>(Lifetime.Singleton).As<ILootSource, IDisposable>();
    }

    protected override void RegisterBattleRegistrators(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
    {
        builder.Register<HealLootBattleRegistrator>(Lifetime.Singleton).As<EntityBattleRegistrator<HealLootRoster>>();
        builder.Register<CurrencyLootBattleRegistrator>(Lifetime.Singleton).As<EntityBattleRegistrator<CurrencyLootRoster>>();
    }
}
