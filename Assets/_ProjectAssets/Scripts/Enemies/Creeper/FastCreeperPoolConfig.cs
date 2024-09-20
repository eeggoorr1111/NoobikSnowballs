using Narratore.Pools;
using UnityEngine;

[System.Serializable]
public class FastCreeperPoolConfig : MBPoolConfig<FastCreeperRoster>
{
    public FastCreeperPoolConfig(FastCreeperRoster sample, Transform poolParent, Transform whenItemActiveParent, FastCreeperRoster[] preInstanced = null, FastCreeperRoster[] rented = null) : base(sample, poolParent, whenItemActiveParent, preInstanced, rented)
    {
    }
}
