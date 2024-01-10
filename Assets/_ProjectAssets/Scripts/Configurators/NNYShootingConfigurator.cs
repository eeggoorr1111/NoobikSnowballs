﻿using Narratore.Input;
using Narratore.Pools;
using Narratore.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;


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
        [SerializeField] private IsShootingWith2Hands _isShootingWith2Hands;

        [Header("INPUT")]
        [SerializeField] private Joystick _joystick;
        [SerializeField] private TouchArea _shootScreenArea;

        [Header("UI")]
        [SerializeField] private TMP_Text _leftBulletsLabel;
        [SerializeField] private Slider _rechargeSlider;
        [SerializeField] private RectTransform _bulletsInfoPanel;
        [SerializeField] private Canvas _playerCanvas;

        [Header("DESKTOP")]
        [SerializeField] private LayerMask _desktopShootLayerMask;

        [Header("RESURRECT")]
        [SerializeField] private float _resurrectShieldDuration = 3f;

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


            builder.Register<ShieldResurrection>(Lifetime.Singleton).WithParameter(_resurrectShieldDuration).AsSelf().AsImplementedInterfaces();
            builder.RegisterEntryPoint<EnemiesPushing>(Lifetime.Singleton).WithParameter<IReadOnlyList<ShootingPushConfig>>(_shootingPushConfig);

            builder.RegisterInstance(_shootScreenArea).As<ITouchArea>();
            builder.RegisterInstance(_isShootingWith2Hands);

            builder.Register<PlayerUnitBattleRegistrator>(Lifetime.Singleton);
            builder.Register<PlayerCharacterMover>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces().WithParameter(_joystick);
            builder.Register<PlayerUnitFacade>(Lifetime.Singleton).AsImplementedInterfaces()
                .WithParameter(_unitSpawner)
                .WithParameter("mainGunSpawner", _mainGunSpawner)
                .WithParameter("secondGunSpawner", _secondGunSpawner)
                .WithParameter(_isShootingWith2Hands);

            builder.RegisterEntryPoint<PlayerBulletsUiObserver>(Lifetime.Singleton).As<IBeginnedUpdatable>()
                .WithParameter(_leftBulletsLabel)
                .WithParameter(_bulletsInfoPanel)
                .WithParameter(_playerCanvas)
                .WithParameter(_rechargeSlider);

            if (config.DeviceType == DeviceType.Desktop)
            {
                builder.Register<DesktopPlayerShooting>(Lifetime.Singleton)
                    .AsImplementedInterfaces();

                builder.Register<DesktopPlayerUnitRotator>(Lifetime.Singleton)
                    .WithParameter(_desktopShootLayerMask)
                    .AsImplementedInterfaces();
            }
            else
            {

            }
        }


        private void OnValidate()
        {
            foreach (var config in _shootingPushConfig)
                config.OnValidate();
        }

        protected override Type GetCombineDamageSource() => typeof(NNYDamageSource);
        protected override Type GetCombineUnitsDeathSource() => typeof(NNYCombineUnitsDeath);
        protected override Type GetCombineExplosionSource() => typeof(NNYExplosionSource);
    }
}

