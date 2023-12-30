using Narratore.MetaGame;
using Narratore.Solutions.Battle;
using UnityEngine;

public class PlayerGunRoster : UpgradableShopItem
{
    public Transform Root => _root;
    public Gun Gun => _gun;
    public LocalPositionRecoil Recoil => _recoil;
    public IntStat Damage => _damage;


    [SerializeField] private Transform _root;
    [SerializeField] private Gun _gun;
    [SerializeField] private LocalPositionRecoil _recoil;
    [SerializeField] private IntStat _damage;
}
