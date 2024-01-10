using Narratore.Pools;
using UnityEngine;


[System.Serializable]
public class BaseCreeperPoolConfig : MBPoolConfig<BaseCreeperRoster>
{
    public BaseCreeperPoolConfig(BaseCreeperRoster sample, Transform poolParent, Transform whenItemActiveParent, int startItemsCount = 0, bool isActivateDeactivateItem = true) : base(sample, poolParent, whenItemActiveParent, startItemsCount, isActivateDeactivateItem)
    {
    }

    public BaseCreeperPoolConfig(BaseCreeperRoster sample, Transform poolParent, Transform whenItemActiveParent, BaseCreeperRoster[] startItems = null, bool isActivateDeactivateItem = true) : base(sample, poolParent, whenItemActiveParent, startItems, isActivateDeactivateItem)
    {
    }
}
