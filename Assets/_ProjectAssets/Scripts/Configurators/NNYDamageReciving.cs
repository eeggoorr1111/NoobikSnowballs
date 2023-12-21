using Narratore.Solutions.Battle;

namespace Narratore.DI
{
    public class NNYDamageReciving : MultyDamageReciving
    {
        public NNYDamageReciving(ShootingDamageReciving shooting, ExplosionDamageReciving explosion, IReadDeadUnitsIds deadUnitsIds) : base(deadUnitsIds)
        {
            TryAdd(shooting);
            TryAdd(explosion);
        }
    }
}

