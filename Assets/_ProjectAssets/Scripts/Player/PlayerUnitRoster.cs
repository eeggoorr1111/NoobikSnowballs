using Narratore.MetaGame;
using Narratore.Solutions.Battle;
using UnityEngine;


public class PlayerUnitRoster : UpgradableShopItem
{
    public Transform Root => _root;
    public Transform GunRecoilTarget => _gunRecoilTarget;
    public MovableBounds Bounds => _bounds;
    public PlayerGunSpawner GunSpawner => _gunSpawner;


    [SerializeField] private Transform _root;
    [SerializeField] private Transform _gunRecoilTarget;
    [SerializeField] private MovableBounds _bounds;
    [SerializeField] private PlayerGunSpawner _gunSpawner;
}