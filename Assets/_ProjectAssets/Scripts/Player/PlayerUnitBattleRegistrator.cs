using Narratore.Solutions.Battle;
using UnityEngine;

public class PlayerUnitBattleRegistrator : EntityBattleRegistrator<PlayerUnitBattleRoster>
{
    public PlayerUnitBattleRegistrator(PlayerEntitiesIds playerUnitsIds,
                                        EntitiesAspects<EntityRoster> entities,
                                        EntitiesAspects<Hp> hps,
                                        EntitiesAspects<ReadHp> readHps,
                                        EntitiesAspects<Transform> transforms,
                                        EntitiesListsBounds bounds,
                                        LootCollectors lootCollectors) : base(playerUnitsIds, entities, transforms)
    {
        _hps = hps;
        _readHps = readHps;
        _transforms = transforms;
        _bounds = bounds;
        _lootCollectors = lootCollectors;
    }


    private readonly EntitiesAspects<Hp> _hps;
    private readonly EntitiesAspects<ReadHp> _readHps;
    private readonly EntitiesAspects<Transform> _transforms;
    private readonly EntitiesListsBounds _bounds;
    private readonly LootCollectors _lootCollectors;

    protected override void RegisterImpl(PlayerUnitBattleRoster unit)
    {
        _hps.Set(unit.Id, unit.Hp);
        _readHps.Set(unit.Id, unit.Hp);
        _transforms.Set(unit.Id, unit.Root);
        _bounds.Set(unit.Id, unit.Bounds);
        _lootCollectors[unit.LootCollider] = unit.Id;
    }

    protected override void UnregisterImpl(PlayerUnitBattleRoster unit)
    {
        _hps.Remove(unit.Id);
        _readHps.Remove(unit.Id);
        _transforms.Remove(unit.Id);
        _bounds.Remove(unit.Id);
        _lootCollectors.Remove(unit.LootCollider);
    }
}
