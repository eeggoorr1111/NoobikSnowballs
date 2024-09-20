using Narratore;
using Narratore.DI;
using Narratore.MetaGame;
using Narratore.Solutions.Battle;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class PlayerUnitConfigurator : ShopItemEntitiesConfigurator<NNY_BattleData>
{
    public PlayerUnitFacade SpawnFacade { get; private set; }


    [SerializeField] private PlayerUnitPoolConfig _pool;
    [SerializeField] private ParticleSystem _spawnVfx;
    [SerializeField] private IsShootingWith2Hands _isShootingWith2Hands;

    protected override IReadOnlyList<ISpawner<UpgradableShopItem>> CreateShopSpawners(NNY_BattleData data, EntitiesConfiguratorConfig config)
    {
        SpawnFacade = new PlayerUnitFacade(_spawnVfx, config.Equipping, _isShootingWith2Hands);
        config.Builder.RegisterInstance(SpawnFacade).AsImplementedInterfaces();

        PlayerUnitBattleRegistrator reg = new PlayerUnitBattleRegistrator(data, config.Registrators, SpawnFacade);
        return ToList(PoolSpawner(reg, data, _pool, _ownerPlayerId));
    }

    protected override void OnDestroyImpl()
    {
        SpawnFacade?.Dispose();
        base.OnDestroyImpl();
    }
}
