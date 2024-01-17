﻿using Narratore.Input;
using Narratore.Pools;
using Narratore.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Narratore.DI
{
    public class NNYInput : Configurator
    {
        [SerializeField] private Joystick _moveJoystick;
        [SerializeField] private Joystick _rotateJoystick;
        [SerializeField] private TouchArea _gameTouchArea;
        [SerializeField] private CustomCursor _customCursor;
        [SerializeField] private GameObject _tapToStartLabel;

        [Header("DESKTOP")]
        [SerializeField] private LayerMask _desktopShootLayerMask;


        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            if (enabled)
            {
                if (config.DeviceType == DeviceType.Desktop)
                {
                    _customCursor.enabled = true;
                    _moveJoystick.SetAxisMode();
                    _moveJoystick.ViewJoystick = Joystick.ViewOfJoystick.AlwaysHide;
                    _rotateJoystick.ViewJoystick = Joystick.ViewOfJoystick.AlwaysHide;
                }
                else if (config.DeviceType == DeviceType.Handheld)
                {
                    _customCursor.enabled = false;
                    _moveJoystick.SetTouchMode();
                    _moveJoystick.ViewJoystick = Joystick.ViewOfJoystick.AlwaysShow;
                    _rotateJoystick.ViewJoystick = Joystick.ViewOfJoystick.AlwaysShow;
                }
                   
                if (config.DeviceType == DeviceType.Desktop)
                {
                    builder.Register<DesktopPlayerShooting>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(_gameTouchArea);
                    builder.Register<DesktopPlayerUnitRotator>(Lifetime.Singleton)
                        .WithParameter(_desktopShootLayerMask)
                        .AsImplementedInterfaces();
                }
                else
                {
                    builder.Register<MobilePlayerUnitRotator>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(_rotateJoystick);
                    builder.Register<MobilePlayerShooting>(Lifetime.Singleton).AsImplementedInterfaces();
                    builder.RegisterEntryPoint<MobileLevelStarter>(Lifetime.Singleton)
                        .WithParameter(_gameTouchArea)
                        .WithParameter(_tapToStartLabel);
                }


                builder.Register<PlayerCharacterJoystickMover>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces().WithParameter(_moveJoystick);
            }
        }
    }
}
