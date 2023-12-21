using Narratore.Solutions.Battle;

public class NNYCombineUnitsDeath : CombineUnitsDeath
{
    public NNYCombineUnitsDeath(ShootingUnitsDeath shooting, ExplosionUnitsDeath explosion, DeadUnitsIds deadUnitsIds) : base(deadUnitsIds)
    {
        TryAdd(shooting);
        TryAdd(explosion);
    }
}
