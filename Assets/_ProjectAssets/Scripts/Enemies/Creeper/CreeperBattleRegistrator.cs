using Narratore.DI;
using Narratore.Solutions.Battle;
using UnityEngine;

public sealed class CreeperBattleRegistrator : EntityBattleRegistrator<CreeperRoster>
{
    public CreeperBattleRegistrator(    PlayerEntitiesIds playerUnitsIds,
                                        EntitiesAspects<Transform> transforms,
                                        EntitiesAspects<IShootingKillable> shootingKillable,
                                        EntitiesAspects<IExplosionKillable> explosionKillable,
                                        EntitiesAspects<ExplosionUnitDeath> explosionDeath,
                                        EntitiesAspects<MovableBot> bots,
                                        EntitiesListsBounds bounds,
                                        EntitiesAspects<Hp> hps,
                                        EntitiesAspects<ReadHp> readHps,
                                        MultyExplosionSource explosionSource,
                                        EntitiesAspects<CreeperDeathExplosion> creeperDeath,
                                        EntitiesAspects<EntityRoster> entities) : base(playerUnitsIds, entities, transforms)
    {
        _transforms = transforms;
        _shootingKillable = shootingKillable;
        _explosionKillable = explosionKillable;
        _explosionDeath = explosionDeath;
        _bounds = bounds;
        _hps = hps;
        _readHps = readHps;
        _explosionSource = explosionSource;
        _bots = bots;
        _creeperDeath = creeperDeath;
    }


    private readonly EntitiesAspects<IShootingKillable> _shootingKillable;
    private readonly EntitiesAspects<IExplosionKillable> _explosionKillable;
    private readonly EntitiesAspects<ExplosionUnitDeath> _explosionDeath;
    private readonly EntitiesAspects<CreeperDeathExplosion> _creeperDeath;
    private readonly EntitiesAspects<Transform> _transforms;
    private readonly EntitiesAspects<MovableBot> _bots;
    private readonly EntitiesListsBounds _bounds;
    private readonly EntitiesAspects<Hp> _hps;
    private readonly EntitiesAspects<ReadHp> _readHps;
    private readonly MultyExplosionSource _explosionSource;


    protected override void UnregisterImpl(CreeperRoster unit)
    {
        _transforms.Remove(unit.Id);
        _bounds.Remove(unit.Id);
        _readHps.Remove(unit.Id);
        _hps.Remove(unit.Id);
        _shootingKillable.Remove(unit.Id);
        _explosionKillable.Remove(unit.Id);
        _explosionDeath.Remove(unit.Id);
        _bots.Remove(unit.Id);
        _creeperDeath.Remove(unit.Id);

        _explosionSource.Remove(unit.CreeperDeath);
    }

    protected override void RegisterImpl(CreeperRoster unit)
    {
        unit.Hp.Maximize();

        _transforms.Set(unit.Id, unit.Root);
        _bounds.Set(unit.Id, unit.Bounds);
        _readHps.Set(unit.Id, unit.Hp);
        _hps.Set(unit.Id, unit.Hp);
        _shootingKillable.Set(unit.Id, unit.CreeperDeath);
        _explosionKillable.Set(unit.Id, unit.CreeperDeath);
        _explosionDeath.Set(unit.Id, unit.CreeperDeath);
        _creeperDeath.Set(unit.Id, unit.CreeperDeath);
        _bots.Set(unit.Id, unit.Bot);

        _explosionSource.TryAdd(unit.CreeperDeath);
    }
}
