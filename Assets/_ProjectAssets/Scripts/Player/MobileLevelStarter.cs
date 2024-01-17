using Narratore.UI;
using System;
using UnityEngine;
using VContainer.Unity;

public class MobileLevelStarter : IInitializable, IDisposable
{
    public MobileLevelStarter(PlayerCharacterMover mover, TouchArea touchArea, GameObject tapToStartLabel, IPlayerUnitShooting unitShooting)
    {
        _mover = mover;
        _touchArea = touchArea;
        _tapToStartLabel = tapToStartLabel;
        _unitShooting = unitShooting;
    }


    private readonly PlayerCharacterMover _mover;
    private readonly TouchArea _touchArea;
    private readonly GameObject _tapToStartLabel;
    private readonly IPlayerUnitShooting _unitShooting;

    public void Initialize()
    {
        _tapToStartLabel.SetActive(true);
        _touchArea.GettedInput += OnTouched;
    }
    public void Dispose()
    {
        _touchArea.GettedInput -= OnTouched;
    }


    private void OnTouched()
    {
        _touchArea.gameObject.SetActive(false);
        _touchArea.GettedInput -= OnTouched;

        _tapToStartLabel.SetActive(false);
        _mover.SetInput(Vector2.down);

        for (int i = 0; i < _unitShooting.ShootAreas.Count; i++)
            _unitShooting.ShootAreas[i].Show();
    }
}
