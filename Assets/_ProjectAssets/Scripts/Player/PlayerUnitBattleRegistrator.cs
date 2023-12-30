using Narratore.Solutions.Battle;
using UnityEngine;

public class PlayerUnitBattleRegistrator : EntityBattleRegistrator<PlayerUnitBattleRoster>
{
    public PlayerUnitBattleRegistrator(PlayerEntitiesIds playerUnitsIds,
                                        EntitiesAspects<EntityRoster> entities,
                                        EntitiesAspects<Hp> hps,
                                        EntitiesAspects<ReadHp> readHps,
                                        EntitiesAspects<Transform> transforms,
                                        EntitiesAspects<IShootingKillable> shootingKillable,
                                        EntitiesAspects<IExplosionKillable> explosionKillable,
                                        EntitiesListsBounds bounds) : base(playerUnitsIds, entities, transforms)
    {
        _hps = hps;
        _readHps = readHps;
        _transforms = transforms;
        _shootingKillable = shootingKillable;
        _explosionKillable = explosionKillable;
        _bounds = bounds;
    }


    private readonly EntitiesAspects<Hp> _hps;
    private readonly EntitiesAspects<ReadHp> _readHps;
    private readonly EntitiesAspects<Transform> _transforms;
    private readonly EntitiesAspects<IShootingKillable> _shootingKillable;
    private readonly EntitiesAspects<IExplosionKillable> _explosionKillable;
    private readonly EntitiesListsBounds _bounds;


    protected override void RegisterImpl(PlayerUnitBattleRoster unit)
    {
        _hps.Set(unit.Id, unit.Hp);
        _readHps.Set(unit.Id, unit.Hp);
        _transforms.Set(unit.Id, unit.Root);
        _shootingKillable.Set(unit.Id, unit.Death);
        _explosionKillable.Set(unit.Id, unit.Death);
        _bounds.Set(unit.Id, unit.Bounds);
    }

    protected override void UnregisterImpl(PlayerUnitBattleRoster unit)
    {
        _hps.Remove(unit.Id);
        _readHps.Remove(unit.Id);
        _transforms.Remove(unit.Id);
        _shootingKillable.Remove(unit.Id);
        _explosionKillable.Remove(unit.Id);
        _bounds.Remove(unit.Id);
    }
}
