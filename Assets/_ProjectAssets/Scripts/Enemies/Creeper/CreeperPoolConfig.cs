using Narratore.Pools;
using UnityEngine;


[System.Serializable]
public class CreeperPoolConfig : MBPoolConfig<CreeperRoster>
{
    public CreeperPoolConfig(CreeperRoster sample, Transform poolParent, Transform whenItemActiveParent, CreeperRoster[] preInstanced = null, CreeperRoster[] rented = null) : base(sample, poolParent, whenItemActiveParent, preInstanced, rented)
    {
    }
}
