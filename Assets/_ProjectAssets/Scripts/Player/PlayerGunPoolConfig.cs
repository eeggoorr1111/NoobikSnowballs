using Narratore.Pools;
using UnityEngine;

[System.Serializable]
public class PlayerGunPoolConfig : MBPoolConfig<PlayerGunRoster>
{
    public PlayerGunPoolConfig(PlayerGunRoster sample, Transform poolParent, Transform whenItemActiveParent, PlayerGunRoster[] preInstanced = null, PlayerGunRoster[] rented = null) : base(sample, poolParent, whenItemActiveParent, preInstanced, rented)
    {
    }
}
