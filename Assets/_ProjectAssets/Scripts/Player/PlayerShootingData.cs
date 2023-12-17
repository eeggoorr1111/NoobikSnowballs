using Narratore.Solutions.Battle;
using Narratore.MetaGame;
using UnityEngine;

public class PlayerShootingData
{
    public PlayerShootingData(Gun gun, IntStat damage, Transform gunTransform, int playerUnitId, int playerId)
    {
        _gun = gun;
        _damage = damage;
        _gunTransform = gunTransform;
        _playerUnitId = playerUnitId;
        _playerId = playerId;
    }


    public Gun Gun => _gun;
    public IImpact Damage => new ShellDamage(PlayersIds.LocalPlayerId, _damage.Get());
    public Transform GunTransform => _gunTransform;
    public int PlayerUnitId => _playerUnitId;
    public int PlayerId => _playerId;


    private readonly Gun _gun;
    private readonly IntStat _damage;
    private readonly Transform _gunTransform;
    private readonly int _playerUnitId;
    private readonly int _playerId;
}
