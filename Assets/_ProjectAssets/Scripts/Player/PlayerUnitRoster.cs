using Narratore.Components;
using Narratore.MetaGame;
using Narratore.Solutions.Battle.Impacts;
using Narratore.Solutions.Battle;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerUnitRoster : UpgradableShopItem
{
    public override event UnityAction<bool> OutBattle;

    public Transform GunRecoilTarget => _gunRecoilTarget;
    public IReadOnlyList<MovableBounds> Bounds => _bounds;
    public StatValue<float> MoveSpeed => _moveSpeed;
    public TwoLegsLoopedRotators FootsAnimator => _footsAnimator;
    public SecondHandState SecondHandState => _secondHandState;
    public Hp Hp => _hp;
    public EntityDeath Death => _death;
    public Collider LootCollider => _lootCollider;
    public EntityImpactsMods ProtectMods => _protectMods;
    public EntityImpactsMods AttackMods => _attackMods;
    public EntityEquipment Equipment => _equipment;


    [SerializeField] private Transform _gunRecoilTarget;
    [SerializeField] private MovableBounds[] _bounds;
    [SerializeField] private StatValue<float> _moveSpeed;
    [SerializeField] private TwoLegsLoopedRotators _footsAnimator;
    [SerializeField] private SecondHandState _secondHandState;
    [SerializeField] private Hp _hp;
    [SerializeField] private EntityDeath _death;
    [SerializeField] private Collider _lootCollider;
    [SerializeField] private EntityImpactsMods _protectMods;
    [SerializeField] private EntityImpactsMods _attackMods;
    [SerializeField] private EntityEquipment _equipment;

    public override void ToOutBattle() => OutBattle?.Invoke(true);
}