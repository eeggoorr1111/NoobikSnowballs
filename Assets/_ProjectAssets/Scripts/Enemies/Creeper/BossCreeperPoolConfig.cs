using Narratore.Pools;
using UnityEngine;

[System.Serializable]
public class BossCreeperPoolConfig : MBPoolConfig<BossCreeperRoster>
{
    public BossCreeperPoolConfig(BossCreeperRoster sample, Transform poolParent, Transform whenItemActiveParent, BossCreeperRoster[] preInstanced = null, BossCreeperRoster[] rented = null) : base(sample, poolParent, whenItemActiveParent, preInstanced, rented)
    {
    }
}
