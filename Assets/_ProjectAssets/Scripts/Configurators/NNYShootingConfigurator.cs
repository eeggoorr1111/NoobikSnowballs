using Narratore.Helpers;
using Narratore.Input;
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
        [SerializeField] private Joystick _joystick;

        [Header("DESKTOP")]
        [SerializeField] private LayerMask _desktopShootLayerMask;

        public override void Configure(IContainerBuilder builder, LevelConfig config)
        {
            base.Configure(builder, config);


            if (config.DeviceType == DeviceType.Desktop)
                _joystick.SetAxisMode();
            else
                _joystick.SetTouchMode();

            if (!_unitSpawner.TrySpawn() || !_unitSpawner.Current.GunSpawner.TrySpawn()) return;

            PlayerUnitRoster unitRoster = _unitSpawner.Current;
            PlayerGunRoster gunRoster = unitRoster.GunSpawner.Current;
            PlayerMover mover = new PlayerMover(_joystick, unitRoster.Root, unitRoster.MoveSpeed);

            gunRoster.Recoil.SetTarget(unitRoster.GunRecoilTarget);

            builder.RegisterInstance(new PlayerShootingData(gunRoster.Gun, gunRoster.Damage, unitRoster.Root, IDGenerator.NewID(), PlayersIds.LocalPlayerId));
            builder.RegisterInstance(Camera.main);
            builder.RegisterInstance(mover).As<ITickable, IUnitRotator>();

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

