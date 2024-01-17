using Narratore;
using Narratore.Extensions;
using Narratore.Input;
using UnityEngine;



public class MobilePlayerUnitRotator : IBeginnedUpdatable
{
    public MobilePlayerUnitRotator(Joystick joystick, IPlayerUnitRotator unitRotator)
    {
        _joystick = joystick;
        _unitRotator = unitRotator;
    }


    private readonly Joystick _joystick;
    private readonly IPlayerUnitRotator _unitRotator;

    public void Tick()
    {
        if (_joystick.TryMoveStick(out Vector2 offset, true))
            _unitRotator.Rotate(offset.To3D(Narratore.Enums.TwoAxis.XZ).normalized);
    }
}
