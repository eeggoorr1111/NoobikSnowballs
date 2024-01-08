using Narratore.Solutions.Battle;
using System.Collections.Generic;
using UnityEngine;

public class NNYLootDroping : LootDroping
{
    public NNYLootDroping(LootDeathSources death, IEntitiesAspects<IDropLootData> dropData, IReadOnlyList<ILootSource> sources, int lootOwnerPlayer, IEntitiesAspects<Transform> transforms) : 
        base(death, dropData, sources, lootOwnerPlayer, transforms)
    {
    }
}
