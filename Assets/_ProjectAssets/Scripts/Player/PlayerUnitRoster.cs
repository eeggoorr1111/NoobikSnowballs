using Narratore.Data;
using Narratore.MetaGame;
using Narratore.Solutions.Battle;
using UnityEngine;


public interface IPlayerUnitRoot
{
    Transform Root { get; }
}


public class PlayerUnitRoster : UpgradableShopItem, IPlayerUnitRoot
{
    public Transform Root => _root;
    public Transform GunRecoilTarget => _gunRecoilTarget;
    public MovableBounds Bounds => _bounds;
    public PlayerGunSpawner GunSpawner => _gunSpawner;
    public ReadValue<float> MoveSpeed => _moveSpeed;


    [SerializeField] private Transform _root;
    [SerializeField] private Transform _gunRecoilTarget;
    [SerializeField] private MovableBounds _bounds;
    [SerializeField] private PlayerGunSpawner _gunSpawner;
    [SerializeField] private ReadValue<float> _moveSpeed;
}