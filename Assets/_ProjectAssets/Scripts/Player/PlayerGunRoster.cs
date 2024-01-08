using Narratore.MetaGame;
using Narratore.Solutions.Battle;
using Narratore.Solutions.Timer;
using UnityEngine;


public class PlayerGunRoster : UpgradableShopItem
{
    public Transform Root => _root;
    public Gun Gun => _gun;
    public LocalPositionRecoil Recoil => _recoil;
    public IntStat Damage => _damage;
    public FloatStat MoveSpeed => _moveSpeed;
    public IReadOnlyTimer RechargeTimer => _rechargeTimer;


    [SerializeField] private Transform _root;
    [SerializeField] private Gun _gun;
    [SerializeField] private LocalPositionRecoil _recoil;
    [SerializeField] private IntStat _damage;
    [SerializeField] private FloatStat _moveSpeed;
    [SerializeField] private GunRechargeTimer _rechargeTimer;
}
