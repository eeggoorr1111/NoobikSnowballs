using Narratore.Interfaces;
using Narratore.Solutions.Battle;
using Narratore.Extensions;
using System;
using UnityEngine;


public class DesktopPlayerShooting : IUpdatable, IDisposable
{
    public DesktopPlayerShooting(   PlayerShootingData data, 
                                    ShellsLifetime shellsLifetime, 
                                    Camera camera, 
                                    LayerMask layerMask)
    {
        _data = data;
        _shellsLifetime = shellsLifetime;
        _camera = camera;
        _layerMask = layerMask;

        _data.Gun.Shooted += OnShooted;
    }
        


    private readonly PlayerShootingData _data;
    private readonly ShellsLifetime _shellsLifetime;
    private readonly Camera _camera;
    private readonly LayerMask _layerMask;

    public void Tick()
    {
        if (Input.GetMouseButton(0))
        {
            // Исходим из того, что террейн находиться в 0 точке и так как камера под углом 
            // то высоты недостаточно, поэтому умножаем на 2 - такой небольшой костыль)
            float distance = _camera.transform.position.y * 2;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, distance, _layerMask))
            {
                Vector3 direction = (hit.point - _data.GunTransform.position).WithY(0).normalized;
                _data.GunTransform.forward = direction;

                if (_data.Gun.IsCanTodoAction)
                    _data.Gun.Shoot();
            }
        }
    }

    public void Dispose()
    {
        _data.Gun.Shooted -= OnShooted;
    }


    private void OnShooted()
    {
        _shellsLifetime.Shoot(  _data.Gun.CurrentShell,
                                _data.Gun.ShootPoint,
                                _data.Gun.Direction,
                                _data.PlayerId,
                                _data.PlayerUnitId, 
                                _data.Damage);
    }

}
