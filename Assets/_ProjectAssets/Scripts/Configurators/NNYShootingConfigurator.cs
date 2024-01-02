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
        [SerializeField] private PlayerGunSpawner _mainGunSpawner;
        [SerializeField] private PlayerGunSpawner _secondGunSpawner;
        [SerializeField] private ReadBoolProvider _isShootingWith2Hands;

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

            if (!_unitSpawner.TrySpawn())
                throw new Exception("Error frist spawn unit");

            if (!_mainGunSpawner.TrySpawn())
                throw new Exception("Error frist spawn gun");


            builder.RegisterEntryPoint<EnemiesPushing>(Lifetime.Singleton).WithParameter<IReadOnlyList<ShootingPushConfig>>(_shootingPushConfig);
            builder.RegisterInstance(_shootScreenArea).As<ITouchArea>();


            builder.Register<PlayerUnitBattleRegistrator>(Lifetime.Singleton);
            builder.Register<PlayerCharacterMover>(Lifetime.Singleton).AsSelf().As<ITickable, IPlayerUnitRotator>().WithParameter(_joystick);
            builder.Register<PlayerUnitFacade>(Lifetime.Singleton).As<IPlayerUnitRoot, IPlayerMovableUnit, IPlayerUnitShooting, IDisposable>()
                .WithParameter(_unitSpawner)
                .WithParameter("mainGunSpawner", _mainGunSpawner)
                .WithParameter("secondGunSpawner", _secondGunSpawner)
                .WithParameter(_isShootingWith2Hands);

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

