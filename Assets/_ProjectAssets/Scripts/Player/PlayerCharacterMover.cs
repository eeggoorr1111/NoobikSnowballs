using Narratore;
using Narratore.CameraTools;
using Narratore.Enums;
using Narratore.Extensions;
using Narratore.Input;
using System;
using UnityEngine;

public interface IPlayerUnitRotator
{
    void Rotate(Vector3 forward);
}

public interface IPlayerLastMoveDirection
{
    Vector3 LastMoveDirection { get; }
}

public interface ICameraMover
{
    void UpdateCameraPos();
}


public class PlayerCharacterMover : IPreparedUpdatable, IPlayerUnitRotator, IPlayerLastMoveDirection, ICameraMover
{
    public event Action Moved;


    public PlayerCharacterMover(Joystick joystick, IPlayerMovableUnit unit, ICurrentCameraGetter camera)
    {
        _joystick = joystick;
        _unit = unit;
        _camera = camera;
        _cameraOffset = _camera.Position - _unit.Root.position;

        LastMoveDirection = Vector3.forward;
    }


    public Vector3 LastMoveDirection { get; private set; }


    private readonly Joystick _joystick;
    private readonly IPlayerMovableUnit _unit;
    private readonly ICurrentCameraGetter _camera;
    private readonly Vector3 _cameraOffset;
    private bool _isMoving;
    

    public void UpdateCameraPos() => _camera.Transform.position = _unit.Root.position + _cameraOffset;
    public void Tick()
    {
        if (_joystick.TryMoveStick(out Vector2 offset, true))
        {
            Vector3 direction = offset.To3D(TwoAxis.XZ, 0).normalized;
            Vector3 cachePosition = _unit.Root.position;

            _unit.Root.position += direction * _unit.MoveSpeed.Get() * Time.deltaTime;
            UpdateCameraPos();

            if (!_isMoving)
            {
                _unit.FootsAnimator.Enable();
                _isMoving = true;
            }

            LastMoveDirection = (_unit.Root.position - cachePosition).normalized;
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
