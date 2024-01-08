using Narratore.Solutions.Battle;
using UnityEngine;

namespace Narratore.DI
{
    public class CreeperDeathExplosion : ExplosionUnitDeath
    {
        public void Death()
        {
            /// Немного костыль) Нужно чтобы при самоуничтожении первый раз отправлялось событие о взрыве
            IShootingKillable shootingKillable = this;
            shootingKillable.Death(Vector3.zero);
        }
    }
}

