using Narratore.Data;
using Narratore.Components;
using UnityEngine;
using Narratore.Solutions.Battle;
using System;

public interface IPlayerUnitRoot
{
    Transform Root { get; }
}

public interface IPlayerMovableUnit : IPlayerUnitRoot
{
    ReadValue<float> MoveSpeed { get; }
    LoopedRotators FootsAnimator { get; }
}

public interface IPlayerUnitShooting
{
    event Action Shooted;


    IImpact Damage { get; }
    Vector3 GunDirection { get; }
    Vector3 ShootPoint { get; }
    Shell Shell { get; }
    Vector3 Position { get; }
    int PlayerId { get; }
    int PlayerUnitId { get; }


    void TryShoot();
}


/// <summary>
/// Скрывает сложность того, что юнит и пушка являются респавнящимися
/// </summary>
public class PlayerUnitFacade : IPlayerMovableUnit, IPlayerUnitShooting, IDisposable
{
    public event Action Shooted;


    public PlayerUnitFacade(PlayerUnitSpawner unit, PlayerGunSpawner gunSpawner, PlayerUnitBattleRegistrator registrator)
    {
        _unitSpawner = unit;
        _gunSpawner = gunSpawner;
        _registrator = registrator;
        _playerId = PlayersIds.LocalPlayerId;

        _unitSpawner.Spawned += OnRespawnedUnit;
        _unitSpawner.Spawning += OnRespawningUnit;
        _gunSpawner.Spawned += OnRespawnedGun;

        OnRespawnedGunWithoutCommonRespawn();
        OnRespawnedUnit();
    }


    private readonly PlayerUnitSpawner _unitSpawner;
    private readonly PlayerGunSpawner _gunSpawner;
    private readonly PlayerUnitBattleRegistrator _registrator;
    private readonly int _playerId;

    private PlayerUnitBattleRoster _unit;
    private PlayerGunRoster _gun;

    public Transform Root => _unit.Root;
    public LoopedRotators FootsAnimator => _unit.FootsAnimator;
    public ReadValue<float> MoveSpeed => _unit.MoveSpeed;
    public IImpact Damage => new ShellDamage(PlayersIds.LocalPlayerId, _gunSpawner.Current.Damage.Get());
    public Vector3 GunDirection => _gun.Gun.Direction;
    public Vector3 ShootPoint => _gun.Gun.ShootPoint;
    public bool IsCanTodoAction => _gun.Gun.IsCanTodoAction;
    public Shell Shell => _gun.Gun.CurrentShell;
    public Vector3 Position => Root.position;
    public int PlayerId => _playerId;
    public int PlayerUnitId => _unit.Id;


    public void TryShoot()
    {
        if (_gun.Gun.IsCanTodoAction)
            _gun.Gun.Shoot();
    }

    public void Dispose()
    {
        _unitSpawner.Spawned -= OnRespawnedUnit;
        _unitSpawner.Spawning -= OnRespawningUnit;
        _gunSpawner.Spawned -= OnRespawnedGun;

        if (_gun != null)
            _gun.Gun.Shooted -= OnShooted;
    }


    private void OnRespawnedGun()
    {
        OnRespawnedGunWithoutCommonRespawn();
        OnRespawned();
    }

    private void OnRespawnedGunWithoutCommonRespawn()
    {
        if (_gun != null)
            _gun.Gun.Shooted -= OnShooted;

        _gun = _gunSpawner.Current;
        _gun.Gun.Shooted += OnShooted;
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
            _gun.Root.SetParent(null);
        }
    }

    private void OnRespawned()
    {
        _gun.Recoil.SetTarget(_unit.GunRecoilTarget);
        _gun.Root.SetParent(_unit.GunAttach);

        _gun.Root.localPosition = Vector3.zero;
        _gun.Root.localRotation = Quaternion.identity;
    }

    private void OnShooted() => Shooted?.Invoke();
}
