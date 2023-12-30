using Narratore.Data;
using Narratore.Enums;
using Narratore.Extensions;
using Narratore.Input;
using Narratore.Interfaces;
using UnityEngine;
using Narratore.CameraTools;
using System;
using Narratore.Components;

public interface IPlayerUnitRotator
{
    void Rotate(Vector3 forward);
}


public class PlayerCharacterMover : IUpdatable, IPlayerUnitRotator
{
    public event Action Moved;


    public PlayerCharacterMover(Joystick joystick, IPlayerMovableUnit unit, ICurrentCameraGetter camera)
    {
        _joystick = joystick;
        _unit = unit;
        _camera = camera;
        _cameraOffset = _camera.Position - _unit.Root.position;
    }


    private readonly Joystick _joystick;
    private readonly IPlayerMovableUnit _unit;
    private readonly ICurrentCameraGetter _camera;
    private readonly Vector3 _cameraOffset;
    private bool _isMoving;
    

    public void Tick()
    {
        if (_joystick.TryMoveStick(out Vector2 offset, true))
        {
            Vector3 direction = offset.To3D(TwoAxis.XZ, 0).normalized;

            _unit.Root.position += direction * _unit.MoveSpeed.Get() * Time.deltaTime;
            _camera.Transform.position = _unit.Root.position + _cameraOffset;

            if (!_isMoving)
            {
                _unit.FootsAnimator.Enable();
                _isMoving = true;
            }

            Moved?.Invoke();
        }
        else if (_isMoving)
        {
            _unit.FootsAnimator.Disable();
            _isMoving = false;
        }
    }

    public void Rotate(Vector3 forward)
    {
        _unit.Root.forward = forward;
    }
}
