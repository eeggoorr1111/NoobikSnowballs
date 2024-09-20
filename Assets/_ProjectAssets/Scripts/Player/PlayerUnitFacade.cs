using Narratore.Components;
using Narratore.Data;
using Narratore.Solutions.Battle;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerUnitRoot
{
    Transform Root { get; }
    int EntityId { get; }
}

public interface IPlayerUnitRootAndHp : IPlayerUnitRoot
{
    Hp Hp { get; }
}

public interface IPlayerMovableUnit : IPlayerUnitRoot
{
    MBReadValue<float> MoveSpeed { get; }
    TwoLegsLoopedRotators FootsAnimator { get; }
    bool IsCanMove { get; }
}

public interface IPlayerPushableUnit : IPlayerUnitRoot
{
    bool IsCanMove { get; set; }
}
public interface IPlayerUnitShooting
{
    event Action<Gun> ShootedGun;
    event Action Shooted;
    event Action RechargeTick;
    event Action Recharged;


    Vector3 Position { get; }
    int PlayerId { get; }
    int PlayerUnitId { get; }
    int LeftBullets { get; }
    int MaxBullets { get; }
    float RechargeProgress { get; }
    float MaxDistance { get; }


    void TryShoot();
    void Recharge();

}
/// <summary>
/// Скрывает сложность того, что юнит и пушка являются респавнящимися, а также то, что пушки может быть 2
/// </summary>
public class PlayerUnitFacade : IPlayerUnitRoot, 
                                IPlayerMovableUnit, 
                                IPlayerUnitShooting, 
                                IPlayerPushableUnit, 
                                IPlayerUnitRootAndHp, 
                                IWithHp, 
                                IDisposable
{
    public event Action<Gun> ShootedGun;
    public event Action Shooted;
    public event Action RechargeTick;
    public event Action Recharged;
    public event Action DecreasedHp;
    public event Action ChangedHp;

    public PlayerUnitFacade(ParticleSystem respawnedVfx, EntityEquipment.Equiping equiping, IsShootingWith2Hands is2Guns)
    {
        _guns = new(2);
        _respawnedVfx = respawnedVfx;
        _equiping = equiping;
        _is2Guns = is2Guns;
    }


    private readonly ParticleSystem _respawnedVfx;
    private readonly EntityEquipment.Equiping _equiping;
    private readonly IsShootingWith2Hands _is2Guns;
    private PlayerUnitRoster _unit;
    private List<PlayerGunRoster> _guns;


    public bool Is2Guns => _is2Guns.Value;
    public int Id => _unit.Id;
    public Transform Root => _unit.Root;
    public TwoLegsLoopedRotators FootsAnimator => _unit.FootsAnimator;
    public MBReadValue<float> MoveSpeed => _unit.MoveSpeed;
    public Vector3 Position => Root.position;
    public int PlayerId => PlayersIds.LocalPlayerId;
    public int PlayerUnitId => _unit.Id;
    public int EntityId => PlayerUnitId;
    public Hp Hp => _unit.Hp;
    public ReadHp ReadHp => _unit.Hp;
    public bool IsCanMove { get; set; }
    public int LeftBullets => _guns[0].Gun.MagazineSize.Current;
    public int MaxBullets => _guns[0].Gun.MagazineSize.Max;
    public float RechargeProgress => _guns[0].RechargeTimer.Progress;
    public float MaxDistance => _guns[0].Gun.Attack.ShellConfig.MaxDistance;


    public void TryShoot()
    {
        for (int i = 0; i < _guns.Count; i++)
            if (_guns[i].Gun.IsCanTodoAction)
                _guns[i].Gun.Shoot();
    }

    public void Recharge()
    {
        for (int i = 0; i < _guns.Count; i++)
            _guns[i].Gun.Recharge();
    }

    public void SetUnit(PlayerUnitRoster unit)
    {
        TryRemoveUnit();

        _unit = unit;
        _unit.Hp.Decreased += OnDecresedHp;
        _unit.Hp.Changed += OnChangedHp;

        if (_guns.Count > 0)
            OnRespawned();
    }
    public bool TryRemoveUnit()
    {
        if (_unit != null)
        {
            _unit.Hp.Decreased -= OnDecresedHp;
            _unit.Hp.Changed -= OnChangedHp;

            for (int i = 0; i < _guns.Count; i++)
                _equiping.TryUnequip(_unit.Id, _guns[i].Gun);

            return true;
        }

        return false;
    }

    public void Dispose()
    {
        if (_unit != null)
        {
            _unit.Hp.Decreased -= OnDecresedHp;
            _unit.Hp.Changed -= OnChangedHp;
        }

        for (int i = 0; i < _guns.Count; i++)
            _guns[i].Gun.ShootedGun -= OnShooted;
    }

    public void RemoveSecondGun()
    {
        if (_guns.Count == 2)
        {
            _guns[1].Gun.ShootedGun -= OnShooted;
            _guns[1].ToOutBattle();

            _guns.RemoveAt(1);
        }

        OnRespawned();
    }

    public void SetSecondGun(PlayerGunRoster gun)
    {
        if (_guns.Count == 2)
        {
            _guns[1].Gun.ShootedGun -= OnShooted;
            _guns[1].ToOutBattle();

            _guns.RemoveAt(1);
        }

        _guns.Add(gun);
        OnRespawned();
    }

    public void SetMainGun(PlayerGunRoster gun)
    {
        if (_unit == null)
        {
            Debug.LogError("Try set main gun, but unit is null");
            return;
        }

        TryRemoveMainGun();


        _guns.Add(gun);

        _guns[0].RechargeTimer.Ticked += OnRechargeTick;
        _guns[0].RechargeTimer.Elapsed += OnRecharged;
        _guns[0].Gun.ShootedGun += OnShooted;

        _equiping.TryEquip(_unit.Id, gun.Gun.Slots[0], gun.Gun);

        OnRespawned();
    }

    public void TryRemoveMainGun()
    {
        if (_guns.Count > 0)
        {
            _guns[0].RechargeTimer.Ticked -= OnRechargeTick;
            _guns[0].RechargeTimer.Elapsed -= OnRecharged;

            for (int i = 0; i < _guns.Count; i++)
            {
                _guns[i].ToOutBattle();
                _guns[i].Gun.ShootedGun -= OnShooted;
            }
        }

        _guns.Clear();
    }

    private void OnRespawned()
    {
        PlayerGunRoster mainGun = _guns[0];
        SecondHandState.StateKey state = _guns.Count > 1 ? SecondHandState.StateKey.WithGun : SecondHandState.StateKey.Free;

        _unit.SecondHandState.Switch(state);
        _unit.MoveSpeed.SetStat(mainGun.MoveSpeed);

        mainGun.Recoil.SetTarget(_unit.GunRecoilTarget);

        _respawnedVfx.transform.position = _unit.Root.position;
        _respawnedVfx.Play();
    }

    private void OnShooted(Gun gun)
    {
        ShootedGun?.Invoke(gun);
        Shooted?.Invoke();
    }
    private void OnDecresedHp() => DecreasedHp?.Invoke();
    private void OnChangedHp() => ChangedHp?.Invoke();

    private void OnRechargeTick() => RechargeTick?.Invoke();
    private void OnRecharged() => Recharged?.Invoke();
}