using Narratore.Solutions.Battle;


public class NNYCombineUnitsDeath : CombineUnitsDeath
{
    public NNYCombineUnitsDeath(ShootingDeathSource shooting, ExplosionDeathSource explosion, DeadUnitsIds deadUnitsIds) : base(deadUnitsIds)
    {
        TryAdd(shooting);
        TryAdd(explosion);
    }
}
