using Narratore;
using Narratore.DI;
using Narratore.MetaGame;
using Narratore.Solutions.Battle;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSecondGunConfigurator : ShopItemEntitiesConfigurator<NNY_BattleData>
{
    [SerializeField] private PlayerGunPoolConfig[] _guns;
    [SerializeField] private PlayerUnitConfigurator _unit;

    protected override IReadOnlyList<ISpawner<UpgradableShopItem>> CreateShopSpawners(NNY_BattleData data, EntitiesConfiguratorConfig config)
    {
        List<ISpawner<UpgradableShopItem>> spawners = new List<ISpawner<UpgradableShopItem>>();
        PlayerGunBattleRegistrator reg = new PlayerSecondGunBattleRegistrator(data, config.Registrators, _unit.SpawnFacade);

        for (int i = 0; i < _guns.Length; i++)
            spawners.Add(PoolSpawner(reg, data, _guns[i], _ownerPlayerId));

        return spawners;
    }
}
