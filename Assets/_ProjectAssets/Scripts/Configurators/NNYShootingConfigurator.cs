using Narratore.Helpers;
using Narratore.MetaGame;
using Narratore.Solutions.Battle;
using System;
using UnityEngine;
using VContainer;
using ITickable = VContainer.Unity.ITickable;

namespace Narratore.DI
{
    public class NNYShootingConfigurator : ShootingConfigurator
    {
        [Header("NNY SHOOTING")]
        [SerializeField] private PlayerUnitSpawner _unitSpawner;

        [Header("DESKTOP")]
        [SerializeField] private LayerMask _desktopShootLayerMask;

        public override void Configure(IContainerBuilder builder, LevelConfig config)
        {
            base.Configure(builder, config);

            if (!_unitSpawner.TrySpawn() || !_unitSpawner.Current.GunSpawner.TrySpawn()) return;

            PlayerUnitRoster unitRoster = _unitSpawner.Current;
            PlayerGunRoster gunRoster = unitRoster.GunSpawner.Current;

            gunRoster.Recoil.SetTarget(unitRoster.GunRecoilTarget);

            builder.RegisterInstance(new PlayerShootingData(gunRoster.Gun, gunRoster.Damage, unitRoster.Root, IDGenerator.NewID(), PlayersIds.LocalPlayerId));
            builder.RegisterInstance(Camera.main);

            if (config.DeviceType == DeviceType.Desktop)
            {
                builder.Register<DesktopPlayerShooting>(Lifetime.Singleton)
                    .WithParameter(_desktopShootLayerMask)
                    .As<ITickable, IDisposable>();
                
            }
            else
            {

            }
        }
    }
}

