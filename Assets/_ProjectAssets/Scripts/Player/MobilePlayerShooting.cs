using Narratore;
using Narratore.Solutions.Battle;
using System.Collections.Generic;
using UnityEngine;

public class MobilePlayerShooting : IPreparedUpdatable
{
    public MobilePlayerShooting(IEntitiesAspects<Transform> transfomrs, IPlayerUnitShooting playerUnitShooting, PlayerShooting shooting, PlayerEntitiesIds ids, IEntitiesListsBounds bounds)
    {
        _transfomrs = transfomrs;
        _playerUnitShooting = playerUnitShooting;
        _shooting = shooting;
        _ids = ids;
        _bounds = bounds;
    }


    private readonly IEntitiesAspects<Transform> _transfomrs;
    private readonly IEntitiesListsBounds _bounds;
    private readonly IPlayerUnitShooting _playerUnitShooting;
    private readonly PlayerEntitiesIds _ids;
    private readonly PlayerShooting _shooting;

   
    public void Tick()
    {
        bool isShoot = false;

        foreach (var pair in _transfomrs.All)
        {
            int entityId = pair.Key;
            if (_ids.TryGetOwner(entityId, out int ownerId) && ownerId == PlayersIds.GetBotId(1) && 
                _bounds.TryGet(entityId, out IReadOnlyList<MovableBounds> bounds))
            {
                if (IsUnitInShootArea(pair.Value, bounds[0]))
                {
                    isShoot = true;
                    break;
                }
            }
        }

        _shooting.SetInput(isShoot);
    }


    private bool IsUnitInShootArea(Transform unitTransf, MovableBounds bounds)
    {
        float radius = Mathf.Max(bounds.Bounds.size.x, bounds.Bounds.size.z) / 2;
        for (int i = 0; i < _playerUnitShooting.ShootAreas.Count; i++)
        {
            ShootArea area = _playerUnitShooting.ShootAreas[i];
            if (area.IsInside(unitTransf.position, radius))
                return true;
        }

        return false;
    }
}
