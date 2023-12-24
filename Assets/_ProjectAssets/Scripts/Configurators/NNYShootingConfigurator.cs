using Narratore.Helpers;
using Narratore.Input;
using Narratore.Solutions.Battle;
using Narratore.UnityUpdate;
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

        public override void Configure(IContainerBuilder builder, LevelConfig config, Updatables prepared, Updatables beginned)
        {
            base.Configure(builder, config, prepared, beginned);

            if (config.DeviceType == DeviceType.Desktop)
                _joystick.SetAxisMode();
            else
                _joystick.SetTouchMode();

            if (!_unitSpawner.TrySpawn() || !_unitSpawner.Current.GunSpawner.TrySpawn()) return;

            PlayerUnitRoster unitRoster = _unitSpawner.Current;
            PlayerGunRoster gunRoster = unitRoster.GunSpawner.Current;

            gunRoster.Recoil.SetTarget(unitRoster.GunRecoilTarget);

            builder.RegisterInstance(unitRoster).As<IPlayerUnitRoot, IPlayerMovableUnit>();

            builder.RegisterInstance(new PlayerShootingData(gunRoster.Gun, gunRoster.Damage, unitRoster.Root, IDGenerator.NewID(), PlayersIds.LocalPlayerId));
            builder.Register<PlayerCharacterMover>(Lifetime.Singleton).As<ITickable, IUnitRotator>().WithParameter(_joystick);
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


        protected override Type GetCombineDamageReciving() => typeof(NNYDamageReciving);
        protected override Type GetCombineUnitsDeathSource() => typeof(NNYCombineUnitsDeath);
        protected override Type GetCombineExplosionSource() => typeof(NNYExplosionSource);
    }
}

