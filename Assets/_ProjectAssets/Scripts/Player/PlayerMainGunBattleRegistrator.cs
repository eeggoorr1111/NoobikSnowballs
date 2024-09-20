using Narratore.DI;
using Narratore.MetaGame;

public class PlayerMainGunBattleRegistrator : PlayerGunBattleRegistrator
{
    public PlayerMainGunBattleRegistrator(BattleData data, Registrators registrators, PlayerUnitFacade unit, ShopItemSpawners secondGunSpawner) : base(data, registrators)
    {
        _unit = unit;
        _secondGunSpawner = secondGunSpawner;
    }


    private readonly PlayerUnitFacade _unit;
    private readonly ShopItemSpawners _secondGunSpawner;


    protected override void RegisterImpl(PlayerGunRoster entity, int playerId, IReadHeldPoint spawnPoint = null)
    {
        base.RegisterImpl(entity, playerId, spawnPoint);

        _unit.SetMainGun(entity);

        if (_unit.Is2Guns)
            _secondGunSpawner.Spawn(entity.Key, playerId, null, _unit.Id);
    }

    protected override void UnregisterImpl(PlayerGunRoster entity, bool isClear)
    {
        base.UnregisterImpl(entity, isClear);

        _unit.TryRemoveMainGun();
    }
}
