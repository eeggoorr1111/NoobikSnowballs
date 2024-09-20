using Narratore.DI;

public class PlayerSecondGunBattleRegistrator : PlayerGunBattleRegistrator
{
    public PlayerSecondGunBattleRegistrator(BattleData data, Registrators registrators, PlayerUnitFacade unit) : base(data, registrators)
    {
        _unit = unit;
    }


    private readonly PlayerUnitFacade _unit;


    protected override void RegisterImpl(PlayerGunRoster entity, int playerId, IReadHeldPoint spawnPoint = null)
    {
        base.RegisterImpl(entity, playerId, spawnPoint);

        _unit.SetSecondGun(entity);
    }

    protected override void UnregisterImpl(PlayerGunRoster entity, bool isClear)
    {
        base.UnregisterImpl(entity, isClear);

        _unit.RemoveSecondGun();
    }
}
