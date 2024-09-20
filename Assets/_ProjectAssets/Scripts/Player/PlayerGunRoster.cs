using Narratore.MetaGame;
using Narratore.Solutions.Battle;
using Narratore.Solutions.Timer;
using UnityEngine;
using UnityEngine.Events;


public class PlayerGunRoster : UpgradableShopItem
{
    public override event UnityAction<bool> OutBattle;


    public Gun Gun => _gun;
    public LocalPositionRecoil Recoil => _recoil;
    public FloatStat MoveSpeed => _moveSpeed;
    public IReadOnlyTimer RechargeTimer => _rechargeTimer;


    [Header("PLAYER GUN*")]
    [SerializeField] private Gun _gun;
    [SerializeField] private LocalPositionRecoil _recoil;
    [SerializeField] private FloatStat _moveSpeed;
    [SerializeField] private GunRechargeTimer _rechargeTimer;


    public override void ToOutBattle() => OutBattle?.Invoke(true);
}
