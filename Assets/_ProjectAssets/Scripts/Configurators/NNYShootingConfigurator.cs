using Narratore.Helpers;
using Narratore.Input;
using Narratore.Pools;
using Narratore.Solutions.Battle;
using Narratore.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ITickable = VContainer.Unity.ITickable;

namespace Narratore.DI
{
    public class NNYShootingConfigurator : ShootingConfigurator
    {
        [Header("PUSH CONFIGS")]
        [SerializeField] private ShootingPushConfig[] _shootingPushConfig;

        [Header("NNY SHOOTING")]
        [SerializeField] private PlayerUnitSpawner _unitSpawner;

        [Header("INPUT")]
        [SerializeField] private Joystick _joystick;
        [SerializeField] private TouchArea _shootScreenArea;

        [Header("DESKTOP")]
        [SerializeField] private LayerMask _desktopShootLayerMask;

        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            base.Configure(builder, config, sampleData);

            if (config.DeviceType == DeviceType.Desktop)
                _joystick.SetAxisMode();
            else
                _joystick.SetTouchMode();

            if (!_unitSpawner.TrySpawn() || !_unitSpawner.Current.GunSpawner.TrySpawn()) return;

            PlayerUnitRoster unitRoster = _unitSpawner.Current;
            PlayerGunRoster gunRoster = unitRoster.GunSpawner.Current;

            gunRoster.Recoil.SetTarget(unitRoster.GunRecoilTarget);

            builder.RegisterEntryPoint<EnemiesPushing>(Lifetime.Singleton).WithParameter<IReadOnlyList<ShootingPushConfig>>(_shootingPushConfig);

            builder.RegisterInstance(unitRoster).As<IPlayerUnitRoot, IPlayerMovableUnit>();
            builder.RegisterInstance(_shootScreenArea).As<ITouchArea>();

            builder.RegisterInstance(new PlayerShootingData(gunRoster.Gun, gunRoster.Damage, unitRoster.Root, IDGenerator.NewID(), PlayersIds.LocalPlayerId));
            builder.Register<PlayerCharacterMover>(Lifetime.Singleton).AsSelf().As<ITickable, IUnitRotator>().WithParameter(_joystick);
            builder.RegisterInstance(Camera.main);
            
            if (config.DeviceType == DeviceType.Desktop)
            {
                builder.Register<DesktopPlayerShooting>(Lifetime.Singleton)
                    .WithParameter(_desktopShootLayerMask)
                    .As<ITickable, IDisposable, IPlayerShooting>();
                
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

