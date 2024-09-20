using Narratore.DI;
using Narratore.Solutions.Battle;

public abstract class PlayerGunBattleRegistrator : EntityRoster.BattleRegistrator<PlayerGunRoster>
{
    protected PlayerGunBattleRegistrator(BattleData data, Registrators registrators) : base(data, registrators)
    {
    }


    protected override void RegisterImpl(PlayerGunRoster entity, int playerId, IReadHeldPoint spawnPoint = null)
    {
        _registrators.Weapons.Register(entity.Gun, entity.Id, Weapon.OwnerIs.OwnerOfDirectOwner);
    }

    protected override void UnregisterImpl(PlayerGunRoster entity, bool isClear)
    {
        _registrators.Weapons.Unregister(entity.Gun);
    }
}
