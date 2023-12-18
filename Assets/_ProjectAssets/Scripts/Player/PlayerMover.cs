using Narratore.Data;
using Narratore.Enums;
using Narratore.Extensions;
using Narratore.Input;
using Narratore.Interfaces;
using UnityEngine;


public interface IUnitRotator
{
    void Rotate(Vector3 forward);
}

public class PlayerMover : IUpdatable, IUnitRotator
{
    public PlayerMover(Joystick joystick, Transform unit, ReadValue<float> speed)
    {
        _joystick = joystick;
        _unit = unit;
        _speed = speed;
    }


    private readonly Joystick _joystick;
    private readonly Transform _unit;
    private readonly ReadValue<float> _speed;
    

    public void Tick()
    {
        if (_joystick.TryMoveStick(out Vector2 offset, true))
        {
            Vector3 direction = offset.To3D(TwoAxis.XZ, 0).normalized;

            _unit.position += direction * _speed.Get() * Time.deltaTime;
        }
    }

    public void Rotate(Vector3 forward)
    {
        _unit.forward = forward;
    }
}
