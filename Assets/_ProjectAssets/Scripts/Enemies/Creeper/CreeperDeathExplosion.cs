using Narratore.Solutions.Battle;
using UnityEngine;

namespace Narratore.DI
{
    public class CreeperDeathExplosion : EntityDeath, IPushKillable
    {
        public void Death(Vector3 impulse) => ToDeath(true);
    }
}

