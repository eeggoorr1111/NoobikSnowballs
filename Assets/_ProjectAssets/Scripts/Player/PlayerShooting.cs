using System;
using UnityEngine;


public interface IPlayerShooting
{
    event Action GettedCommandShoot;
}

public class PlayerShooting : IPlayerShooting
{
    public event Action GettedCommandShoot;


    public PlayerShooting(IPlayerUnitShooting shooting)
    {
        _unitShooting = shooting;
    }



    private readonly IPlayerUnitShooting _unitShooting;
    private bool _isShooting;

    public void SetInput(bool isShoot)
    {
        if (isShoot)
        {
            _unitShooting.TryShoot();
            GettedCommandShoot?.Invoke();
        }
        else if (_isShooting)
        {
            _isShooting = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
            _unitShooting.Recharge();
    }
}
