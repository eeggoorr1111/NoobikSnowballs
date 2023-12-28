using Narratore.Data;
using Narratore.DI;
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
    public IShootingKillable ShootingKillable => _death;
    public IExplosionKillable ExplosionKillable => _death;
    public CreeperDeathExplosion CreeperDeath => _death;
    public MovableBot Bot => _bot;


    [SerializeField] private Hp _hp;
    [SerializeField] private MovableBounds[] _bounds;
    [SerializeField] private CreeperDeathExplosion _death;
    [SerializeField] private ReadValue<float> _speed;
    [SerializeField] private MovableBot _bot;
}
