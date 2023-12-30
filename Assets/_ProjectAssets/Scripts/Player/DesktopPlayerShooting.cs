using Narratore.Interfaces;
using Narratore.Solutions.Battle;
using Narratore.Extensions;
using System;
using UnityEngine;
using Narratore.UI;
using Narratore.CameraTools;

public interface IPlayerShooting
{
    event Action GettedCommandShoot;
    event Action StartedShoot;
    event Action EndedShoot;
}


public class DesktopPlayerShooting : IUpdatable, IDisposable, IPlayerShooting
{
    public event Action GettedCommandShoot;
    public event Action StartedShoot;
    public event Action EndedShoot;


    public DesktopPlayerShooting( IPlayerUnitShooting shooting,
                                    ShellsLifetime shellsLifetime,
                                    ICurrentCameraGetter camera,
                                    LayerMask layerMask,
                                    IPlayerUnitRotator unitRotator,
                                    ITouchArea touchArea)
    {
        _unitShooting = shooting;
        _shellsLifetime = shellsLifetime;
        _camera = camera;
        _layerMask = layerMask;
        _unitRotator = unitRotator;
        _touchArea = touchArea;

        _unitShooting.Shooted += OnShooted;
    }



    private readonly IPlayerUnitShooting _unitShooting;
    private readonly ShellsLifetime _shellsLifetime;
    private readonly ICurrentCameraGetter _camera;
    private readonly LayerMask _layerMask;
    private readonly IPlayerUnitRotator _unitRotator;
    private readonly ITouchArea _touchArea;
    private bool _isShooting;

    public void Tick()
    {
        if (_touchArea.IsHold)
        {
            // Исходим из того, что террейн находиться в 0 точке и так как камера под углом 
            // то высоты недостаточно, поэтому умножаем на 2 - такой небольшой костыль)
            float distance = _camera.Transform.position.y * 2;
            Ray ray = _camera.Get.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, distance, _layerMask))
            {
                Vector3 direction = (hit.point - _unitShooting.Position).WithY(0).normalized;

                _unitRotator.Rotate(direction);
                _unitShooting.TryShoot();
            }

            GettedCommandShoot?.Invoke();
        }
        else if (_isShooting)
        {
            _isShooting = false;
            EndedShoot?.Invoke();
        }
    }

    public void Dispose()
    {
        _unitShooting.Shooted -= OnShooted;
    }

    private void OnShooted()
    {
        _shellsLifetime.Shoot(  _unitShooting.Shell,
                                _unitShooting.ShootPoint,
                                _unitShooting.GunDirection,
                                _unitShooting.PlayerId,
                                _unitShooting.PlayerUnitId, 
                                _unitShooting.Damage);

        if (!_isShooting)
        {
            _isShooting = true;
            StartedShoot?.Invoke();
        }
    }
}
