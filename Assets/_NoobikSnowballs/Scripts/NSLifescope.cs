using VContainer;
using VContainer.Unity;
using UnityEngine;
using Narratore.Solutions.Battle;


public class NSLifescope : LifetimeScope
{
    [Header("BATTLE")]
    [SerializeField] ShellShooting _shooting;



    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_shooting).As<ShellShooting, ITickable>();
    }
}
