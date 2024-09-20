using Narratore.Pools;
using UnityEngine;

[System.Serializable]
public class PlayerUnitPoolConfig : MBPoolConfig<PlayerUnitRoster>
{
    public PlayerUnitPoolConfig(PlayerUnitRoster sample, Transform poolParent, Transform whenItemActiveParent, PlayerUnitRoster[] preInstanced = null, PlayerUnitRoster[] rented = null) : base(sample, poolParent, whenItemActiveParent, preInstanced, rented)
    {
    }
}
