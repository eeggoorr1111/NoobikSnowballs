using Narratore;
using Narratore.Abstractions;
using UnityEngine;
using UnityEngine.UI;

public class ShootingWith2HandsBattle : MonoBehaviour
{
    [SerializeField] private BoolProvider _provider;

    private Button _button;


    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }


    private async void OnClick()
    {
        if (RewardedAds.Instance != null && RewardedAds.Instance.TryShow())
        {
            await RewardedAds.Instance.ShowingTask;
            _provider.Set(true);
        }
    }
}
