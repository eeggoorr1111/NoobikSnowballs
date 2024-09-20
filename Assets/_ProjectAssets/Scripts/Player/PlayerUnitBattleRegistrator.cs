using Narratore.DI;
using Narratore.Solutions.Battle;

public class PlayerUnitBattleRegistrator : EntityRoster.BattleRegistrator<PlayerUnitRoster>
{
    public PlayerUnitBattleRegistrator(BattleData data, Registrators registrators, PlayerUnitFacade unit) : base(data, registrators)
    {
        _unit = unit;
    }

    private readonly PlayerUnitFacade _unit;

    protected override void RegisterImpl(PlayerUnitRoster entity, int playerId, IReadHeldPoint spawnPoint = null)
    {
        _data.EntityHp.Set(entity.Id, entity.Hp);
        _data.EntityReadHp.Set(entity.Id, entity.Hp);
        _data.EntityRoot.Set(entity.Id, entity.Root);
        _data.EntityBounds.Set(entity.Id, entity.Bounds);
        _data.EntityProtectMods.Set(entity.Id, entity.ProtectMods);
        _data.EntityAttackMods.Set(entity.Id, entity.AttackMods);
        _data.EntityEquipment.Set(entity.Id, entity.Equipment);

        _unit.SetUnit(entity);
    }

    protected override void UnregisterImpl(PlayerUnitRoster entity, bool isClear)
    {
        _data.EntityHp.Remove(entity.Id);
        _data.EntityReadHp.Remove(entity.Id);
        _data.EntityRoot.Remove(entity.Id);
        _data.EntityBounds.Remove(entity.Id);
        _data.EntityProtectMods.Remove(entity.Id);
        _data.EntityAttackMods.Remove(entity.Id);
        _data.EntityEquipment.Remove(entity.Id);

        _unit.TryRemoveUnit();
    }
}
