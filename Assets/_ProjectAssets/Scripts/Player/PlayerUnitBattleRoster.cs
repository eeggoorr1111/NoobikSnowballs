using Narratore.Data;
using Narratore.MetaGame;
using Narratore.Solutions.Battle;
using Narratore.Components;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlayerUnitBattleRoster : EntityRoster
{
    public override event UnityAction OutBattle
    {
        add { _death.Ended += value; }
        remove { _death.Ended -= value; }
    }


    public Transform GunRecoilTarget => _gunRecoilTarget;
    public Transform GunAttach => _gunAttach;
    public IReadOnlyList<MovableBounds> Bounds => _bounds;
    public ReadValue<float> MoveSpeed => _moveSpeed;
    public LoopedRotators FootsAnimator => _footsAnimator;
    public Hp Hp => _hp;
    public StubUnitDeath Death => _death;


    [SerializeField] private Transform _gunRecoilTarget;
    [SerializeField] private Transform _gunAttach;
    [SerializeField] private MovableBounds[] _bounds;
    [SerializeField] private ReadValue<float> _moveSpeed;
    [SerializeField] private LoopedRotators _footsAnimator;
    [SerializeField] private Hp _hp;
    [SerializeField] private StubUnitDeath _death;
}
