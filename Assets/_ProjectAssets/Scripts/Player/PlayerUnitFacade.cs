using Narratore.Data;
using Narratore.Components;
using UnityEngine;
using Narratore.Solutions.Battle;
using System;
using Narratore;
using System.Collections.Generic;

public interface IPlayerUnitRoot
{
    Transform Root { get; }
}

public interface IPlayerMovableUnit : IPlayerUnitRoot
{
    ReadValue<float> MoveSpeed { get; }
    TwoLegsLoopedRotators FootsAnimator { get; }
}

public interface IPlayerUnitShooting
{
    event Action<Gun> Shooted;


    IImpact Damage { get; }
    Vector3 Position { get; }
    int PlayerId { get; }
    int PlayerUnitId { get; }


    void TryShoot();
}


/// <summary>
/// Скрывает сложность того, что юнит и пушка являются респавнящимися, а также то, что пушки может быть 2
/// </summary>
public class PlayerUnitFacade : IPlayerMovableUnit, IPlayerUnitShooting, IDisposable
{
    public event Action<Gun> Shooted;


    public PlayerUnitFacade(PlayerUnitSpawner unit,
                            PlayerGunSpawner mainGunSpawner,
                            PlayerGunSpawner secondGunSpawner,
                            PlayerUnitBattleRegistrator registrator,
                            ReadBoolProvider isShootingWith2Hand)
    {
        _unitSpawner = unit;
        _mainGunSpawner = mainGunSpawner;
        _registrator = registrator;
        _playerId = PlayersIds.LocalPlayerId;
        _secondGunSpawner = secondGunSpawner;
        _isShootingWith2Hand = isShootingWith2Hand;

        _unitSpawner.Spawned += OnRespawnedUnit;
        _unitSpawner.Spawning += OnRespawningUnit;
        _mainGunSpawner.Spawned += OnRespawnedGun;
        _isShootingWith2Hand.Changed += OnChangedCountHands;

        _guns = new(2);

        OnRespawnedGunWithoutCommonRespawn();
        OnRespawnedUnit();
    }


    private readonly PlayerUnitSpawner _unitSpawner;
    private readonly PlayerGunSpawner _mainGunSpawner;
    private readonly PlayerGunSpawner _secondGunSpawner;
    private readonly PlayerUnitBattleRegistrator _registrator;
    private readonly ReadBoolProvider _isShootingWith2Hand;
    private readonly int _playerId;

    private PlayerUnitBattleRoster _unit;
    private List<PlayerGunRoster> _guns;

    public Transform Root => _unit.Root;
    public TwoLegsLoopedRotators FootsAnimator => _unit.FootsAnimator;
    public ReadValue<float> MoveSpeed => _unit.MoveSpeed;
    public IImpact Damage => new ShellDamage(PlayersIds.LocalPlayerId, _mainGunSpawner.Current.Damage.Get());
    public Vector3 Position => Root.position;
    public int PlayerId => _playerId;
    public int PlayerUnitId => _unit.Id;


    public void TryShoot()
    {
        for (int i = 0; i < _guns.Count; i++)
            if (_guns[i].Gun.IsCanTodoAction)
                _guns[i].Gun.Shoot();
    }

    public void Dispose()
    {
        _unitSpawner.Spawned -= OnRespawnedUnit;
        _unitSpawner.Spawning -= OnRespawningUnit;
        _mainGunSpawner.Spawned -= OnRespawnedGun;
        _isShootingWith2Hand.Changed -= OnChangedCountHands;

        for (int i = 0; i < _guns.Count; i++)
            _guns[i].Gun.ShootedGun -= OnShooted;
    }


    private void OnChangedCountHands() => _mainGunSpawner.TrySpawn();

    private void OnRespawnedGun()
    {
        OnRespawnedGunWithoutCommonRespawn();
        OnRespawned();
    }

    private void OnRespawnedGunWithoutCommonRespawn()
    {
        foreach (var gun in _guns)
            gun.Gun.ShootedGun -= OnShooted;
        _guns.Clear();

        _guns.Add(_mainGunSpawner.Current);
        if (_isShootingWith2Hand.Value && _secondGunSpawner.TrySpawn())
            _guns.Add(_secondGunSpawner.Current);

        foreach (var gun in _guns)
            gun.Gun.ShootedGun += OnShooted;
    }

    private void OnRespawnedUnit()
    {
        _unit = _unitSpawner.Current.BattleRoster;
        _registrator.Register(_unit, _playerId);

        OnRespawned();
    }

    private void OnRespawningUnit()
    {
        if (_unit != null)
        {
            _registrator.Unregister(_unit);

            foreach (var gun in _guns)
                gun.Root.SetParent(null);
        }
    }

    private void OnRespawned()
    {
        PlayerGunRoster mainGun = _guns[0];
        SecondHandState.StateKey state = _guns.Count > 1 ? SecondHandState.StateKey.WithGun : SecondHandState.StateKey.Free;


        mainGun.Root.SetParent(_unit.MainGunAttach);
        mainGun.Root.localPosition = Vector3.zero;
        mainGun.Root.localRotation = Quaternion.identity;

        if (state == SecondHandState.StateKey.WithGun)
        {
            PlayerGunRoster secondGun = _guns[1];

            secondGun.Root.SetParent(_unit.SecondGunAttach);
            secondGun.Root.localPosition = Vector3.zero;
            secondGun.Root.localRotation = Quaternion.identity;
        }

        _unit.SecondHandState.Switch(state);
        _unit.MoveSpeed.SetStat(mainGun.MoveSpeed);
        mainGun.Recoil.SetTarget(_unit.GunRecoilTarget);
    }

    private void OnShooted(Gun gun) => Shooted?.Invoke(gun);
}
