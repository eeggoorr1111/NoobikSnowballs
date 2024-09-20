using Narratore.Pools;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;


namespace Narratore.DI
{
    public class NNYShootingConfigurator : LevelConfigurator
    {
        [Header("PUSH CONFIGS")]
        [SerializeField] private ShootingPushConfig[] _shootingPushConfig;

        [Header("NNY SHOOTING")]
        [SerializeField] private IsShootingWith2Hands _isShootingWith2Hands;

        [Header("UI")]
        [SerializeField] private TMP_Text _leftBulletsLabel;
        [SerializeField] private Slider _rechargeSlider;
        [SerializeField] private RectTransform _bulletsInfoPanel;
        [SerializeField] private Canvas _playerCanvas;

        [Header("RESURRECT")]
        [SerializeField] private float _resurrectShieldDuration = 3f;

        protected override void ConfigureImpl(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            //builder.Register<ShieldResurrection>(Lifetime.Singleton).WithParameter(_resurrectShieldDuration).AsSelf().AsImplementedInterfaces();
            //builder.RegisterEntryPoint<EnemiesPushing>(Lifetime.Singleton).WithParameter<IReadOnlyList<ShootingPushConfig>>(_shootingPushConfig);

            
            builder.RegisterInstance(_isShootingWith2Hands);

            builder.Register<PlayerCharacterMover>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();

            builder.RegisterEntryPoint<PlayerBulletsUiObserver>(Lifetime.Singleton).As<IBeginnedTickable>()
                .WithParameter(_leftBulletsLabel)
                .WithParameter(_bulletsInfoPanel)
                .WithParameter(_playerCanvas)
                .WithParameter(_rechargeSlider);

            builder.Register<PlayerShooting>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }


        private void OnValidate()
        {
            foreach (var config in _shootingPushConfig)
                config.OnValidate();
        }
    }
}

