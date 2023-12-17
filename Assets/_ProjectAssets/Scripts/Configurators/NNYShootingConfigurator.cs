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
        [SerializeField] private Gun _playerGun;
        [SerializeField] private Transform _gunTransform;
        [SerializeField] private IntStat _damage;
        [SerializeField] private int _playerUnitId;

        [Header("DESKTOP")]
        [SerializeField] private LayerMask _desktopShootLayerMask;

        public override void Configure(IContainerBuilder builder, LevelConfig config)
        {
            base.Configure(builder, config);

            builder.RegisterInstance(new PlayerShootingData(_playerGun, _damage, _gunTransform, _playerUnitId, PlayersIds.LocalPlayerId));
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

