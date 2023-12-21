using Narratore.Data;
using Narratore.Solutions.Battle;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreeperRoster : EntityRoster
{
    public override event UnityAction OutBattle
    {
        add { _death.Ended += value; }
        remove { _death.Ended -= value; }
    }


    public Hp Hp => _hp;
    public IReadOnlyList< MovableBounds > Bounds => _bounds;
    public ReadValue<float> Speed => _speed;
    public IShootingUnitDeath ShootingDeath => _death;
    public IExplosionUnitDeath ExplosionDeath => _death;


    [SerializeField] private Hp _hp;
    [SerializeField] private MovableBounds[] _bounds;
    [SerializeField] private ExplosionUnitDeath _death;
    [SerializeField] private ReadValue<float> _speed;
}
