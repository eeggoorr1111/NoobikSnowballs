using Narratore.Input;
using Narratore.Pools;
using Narratore.UI;
using UnityEngine;
using VContainer;


namespace Narratore.DI
{
    public class NNYInput : Configurator
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private TouchArea _shootScreenArea;

        [Header("DESKTOP")]
        [SerializeField] private LayerMask _desktopShootLayerMask;


        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            if (enabled)
            {
                if (config.DeviceType == DeviceType.Desktop)
                    _joystick.SetAxisMode();
                else
                    _joystick.SetTouchMode();

                if (config.DeviceType == DeviceType.Desktop)
                {
                    builder.Register<DesktopPlayerShooting>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(_shootScreenArea);
                    builder.Register<DesktopPlayerUnitRotator>(Lifetime.Singleton)
                        .WithParameter(_desktopShootLayerMask)
                        .AsImplementedInterfaces();
                }
                else
                {

                }


                builder.Register<PlayerCharacterJoystickMover>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces().WithParameter(_joystick);
                builder.RegisterInstance(_shootScreenArea).As<ITouchArea>();
            }
        }
    }
}

