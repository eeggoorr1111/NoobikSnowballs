using Narratore;
using Narratore.CameraTools;
using Narratore.Extensions;
using Narratore.Helpers;
using Narratore.Solutions.Battle;
using Narratore.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;


public class PlayerBulletsUiObserver : IInitializable, IDisposable, IBeginnedUpdatable
{
    public PlayerBulletsUiObserver( IPlayerUnitShooting unit, 
                                    TMP_Text leftBulletsLabel, 
                                    Slider rechargeSlider, 
                                    RectTransform bulletsInfoPanel, 
                                    ICurrentCameraGetter camera, 
                                    Canvas canvas)
    {
        _unit = unit;
        _leftBulletsLabel = leftBulletsLabel;
        _rechargeSlider = rechargeSlider;
        _bulletsInfoPanel = bulletsInfoPanel;
        _camera = camera;
        _canvas = canvas;
    }


    private readonly IPlayerUnitShooting _unit;
    private readonly TMP_Text _leftBulletsLabel;
    private readonly Slider _rechargeSlider;
    private readonly RectTransform _bulletsInfoPanel;
    private readonly ICurrentCameraGetter _camera;
    private readonly Canvas _canvas;



    public void Tick()
    {
        _leftBulletsLabel.text = _unit.LeftBullets.ToString();
        _bulletsInfoPanel.anchoredPosition = (UiHelper.Convert(_unit.Position, _camera.Get, _camera.Transform, _canvas.scaleFactor) + new Vector3(0, 150, 0)).To2D();
    }

    public void Dispose()
    {
        _unit.RechargeTick -= OnTickTimerRecharge;
        _unit.Recharged -= Recharged;
    }

    public void Initialize()
    {
        _unit.RechargeTick += OnTickTimerRecharge;
        _unit.Recharged += Recharged;

        _leftBulletsLabel.text = _unit.MaxBullets.ToString();
    }


    private void OnTickTimerRecharge()
    {
        if (!_rechargeSlider.gameObject.activeSelf)
            _rechargeSlider.gameObject.SetActive(true);

        _rechargeSlider.value = _unit.RechargeProgress;
    }

    private void Recharged()
    {
        _leftBulletsLabel.text = _unit.MaxBullets.ToString();
        _rechargeSlider.gameObject.SetActive(false);
    }
}


public interface IPlayerShooting
{
    event Action GettedCommandShoot;
    event Action StartedShoot;
    event Action EndedShoot;
}

public class DesktopPlayerShooting : IPreparedUpdatable, IDisposable, IPlayerShooting
{
    public event Action GettedCommandShoot;
    public event Action StartedShoot;
    public event Action EndedShoot;


    public DesktopPlayerShooting( IPlayerUnitShooting shooting,
                                  ShellsLifetime shellsLifetime,
                                  ITouchArea touchArea)
    {
        _unitShooting = shooting;
        _shellsLifetime = shellsLifetime;
        _touchArea = touchArea;

        _unitShooting.ShootedGun += OnShooted;
    }



    private readonly IPlayerUnitShooting _unitShooting;
    private readonly ShellsLifetime _shellsLifetime;
    private readonly ITouchArea _touchArea;
    private bool _isShooting;

    public void Tick()
    {
        if (_touchArea.IsHold)
        {
            _unitShooting.TryShoot();
            GettedCommandShoot?.Invoke();
        }
        else if (_isShooting)
        {
            _isShooting = false;
            EndedShoot?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.R))
            _unitShooting.Recharge();
    }

    public void Dispose()
    {
        _unitShooting.ShootedGun -= OnShooted;
    }

    private void OnShooted(Gun gun)
    {
        _shellsLifetime.Shoot(  gun,
                                _unitShooting.PlayerId,
                                _unitShooting.PlayerUnitId,
                                _unitShooting.GetDamage(),
                                _unitShooting.MaxDistance);

        if (!_isShooting)
        {
            _isShooting = true;
            StartedShoot?.Invoke();
        }
    }
}
