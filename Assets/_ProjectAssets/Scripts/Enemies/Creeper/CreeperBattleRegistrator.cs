using Narratore;
using Narratore.DI;
using Narratore.Solutions.Battle;


public class BossCreeperBattleRegistrator : CreeperBattleRegistrator<BossCreeperRoster>
{
    public BossCreeperBattleRegistrator(NNY_BattleData data, Registrators registrators) : base(data, registrators)
    {
    }

    private new readonly NNY_BattleData _data;

    protected override void UnregisterImpl(BossCreeperRoster entity, bool isClear)
    {
        base.UnregisterImpl(entity, isClear);

        _data.EntityBotShoot.Remove(entity.Id);
    }

    protected override void RegisterImpl(BossCreeperRoster entity, int playerId, IReadHeldPoint spawnPoint = null)
    {
        base.RegisterImpl(entity, playerId, spawnPoint);

        _data.EntityBotShoot.Set(entity.Id, entity.Shooting);
    }
}


public class CreeperBattleRegistrator : CreeperBattleRegistrator<CreeperRoster>
{
    public CreeperBattleRegistrator(NNY_BattleData data, Registrators registrators) : base(data, registrators)
    {
    }
}

public abstract class CreeperBattleRegistrator<T> : EntityRoster.BattleRegistrator<T>
    where T : CreeperRoster
{
    protected CreeperBattleRegistrator(NNY_BattleData data, Registrators registrators) : base(data, registrators)
    {

    }


    private new readonly NNY_BattleData _data;

    protected override void RegisterImpl(T entity, int playerId, IReadHeldPoint spawnPoint = null)
    {
        _data.EntityRoot.Set(entity.Id, entity.Root);
        _data.EntityBounds.Set(entity.Id, entity.Bounds);
        _data.EntityHp.Set(entity.Id, entity.Hp);
        _data.EntityReadHp.Set(entity.Id, entity.Hp);
        _data.EntityPushKillable.Set(entity.Id, entity.CreeperDeath);
        _data.EntityDropLootData.Set(entity.Id, entity.DropLoot);
        _data.EntityReadDropLootData.Set(entity.Id, entity.DropLoot);
        _data.EntityMovableBot.Set(entity.Id, entity.Bot);
    }

    protected override void UnregisterImpl(T entity, bool isClear)
    {
        _data.EntityRoot.Remove(entity.Id);
        _data.EntityBounds.Remove(entity.Id);
        _data.EntityHp.Remove(entity.Id);
        _data.EntityReadHp.Remove(entity.Id);
        _data.EntityPushKillable.Remove(entity.Id);
        _data.EntityDropLootData.Remove(entity.Id);
        _data.EntityReadDropLootData.Remove(entity.Id);
        _data.EntityMovableBot.Remove(entity.Id);
    }
}
