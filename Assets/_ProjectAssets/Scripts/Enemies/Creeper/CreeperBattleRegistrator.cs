using Narratore.Solutions.Battle;
using Narratore.UnitsRepository;
using UnityEngine;

public sealed class CreeperBattleRegistrator : EntityBattleRegistrator<CreeperRoster>
{
    public CreeperBattleRegistrator(    PlayerEntitiesIds playerUnitsIds, 
                                        EntitiesAspects<Transform> transforms, 
                                        EntitiesAspects<IShootingUnitDeath> shootingDeath, 
                                        EntitiesAspects<IExplosionUnitDeath> explosionDeath, 
                                        UnitsListsBounds bounds, 
                                        EntitiesAspects<Hp> hps, 
                                        EntitiesAspects<ReadHp> readHps) : base(playerUnitsIds)
    {
        _transforms = transforms;
        _shootingDeath = shootingDeath;
        _explosionDeath = explosionDeath;
        _bounds = bounds;
        _hps = hps;
        _readHps = readHps;
    }


    private readonly EntitiesAspects<IShootingUnitDeath> _shootingDeath;
    private readonly EntitiesAspects<IExplosionUnitDeath> _explosionDeath;
    private readonly EntitiesAspects<Transform> _transforms;
    private readonly UnitsListsBounds _bounds;
    private readonly EntitiesAspects<Hp> _hps;
    private readonly EntitiesAspects<ReadHp> _readHps;


    protected override void UnregisterImpl(CreeperRoster unit)
    {
        _transforms.Remove(unit.Id);
        _bounds.Remove(unit.Id);
        _readHps.Remove(unit.Id);
        _hps.Remove(unit.Id);
        _shootingDeath.Remove(unit.Id);
        _explosionDeath.Remove(unit.Id);
    }

    protected override void RegisterImpl(CreeperRoster unit)
    {
        _transforms.Set(unit.Id, unit.Root);
        _bounds.Set(unit.Id, unit.Bounds);
        _readHps.Set(unit.Id, unit.Hp);
        _hps.Set(unit.Id, unit.Hp);
        _shootingDeath.Set(unit.Id, unit.ShootingDeath);
        _explosionDeath.Set(unit.Id, unit.ExplosionDeath);
    }
}
