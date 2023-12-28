using Narratore.Interfaces;
using Narratore.Solutions.Battle;
using System.Linq;
using UnityEngine;

namespace Narratore.DI
{
    public class CreeperSelfExplosionDeathSource : DeathSource, IUpdatable
    {
        public CreeperSelfExplosionDeathSource( DeadUnitsIds deadUnitsIds, 
                                                IPlayerUnitRoot playerUnit, 
                                                IEntitiesAspects<CreeperDeathExplosion> creepers, 
                                                IEntitiesAspects<Transform> creepersTransform, 
                                                float explosionDistance) : base(deadUnitsIds)
        {
            _playerUnit = playerUnit;
            _creepers = creepers;
            _creepersTransform = creepersTransform;
            _sqrExplosionDistance = explosionDistance * explosionDistance;
        }


        private readonly IPlayerUnitRoot _playerUnit;
        private readonly IEntitiesAspects<CreeperDeathExplosion> _creepers;
        private readonly IEntitiesAspects<Transform> _creepersTransform;
        private readonly float _sqrExplosionDistance;


        public override void Dispose() { }

        public void Tick()
        {
            Vector3 playerPosition = _playerUnit.Root.position;
            int[] ids = _creepers.All.Keys.ToArray();

            for (int i = 0; i < ids.Length; i++)
            {
                int creeperId = ids[i];
                if (!IsDead(creeperId) && IsNearPoint(creeperId, playerPosition) && _creepers.TryGet(creeperId, out CreeperDeathExplosion creeper))
                {
                    ToDeath(creeperId);
                    creeper.Death();
                }
            }
        }


        private bool IsNearPoint(int creeperId, Vector3 point)
        {
            return _creepersTransform.TryGet(creeperId, out Transform transf) &&
                    (transf.position - point).sqrMagnitude < _sqrExplosionDistance;
        }

    }
}

