using Narratore;
using Narratore.Abstractions;
using Narratore.Helpers;
using Narratore.Interfaces;
using Narratore.MetaGame;
using Narratore.Solutions.Battle;
using Narratore.Solutions.Windows;
using Narratore.UI;
using System.Threading;
using UnityEngine;


public sealed class NNYLevelMain : LevelMain
{
    public NNYLevelMain(IUnitsWavesSpawner spawner,
                        LevelConfig config,
                        PlayerCharacterMover playerMover,
                        IPlayerShooting playerShooting,
                        ShopWindow shopWindow,
                        LoseWindow loseWindow,
                        WinWindow winWindow,
                        IWithHp playerUnit,
                        GameStateUpdatables updatables,
                        LevelResultData resultData,
                        WalletProvider wallet,
                        CurrencyDescriptor currency,
                        IsShootingWith2Hands isShootingWith2Hands,
                        GameStateEvents events) : base(config, updatables, events)
    {
        _spawner = spawner;
        _playerMover = playerMover;
        _playerShooting = playerShooting;
        _loseConditionUnit = playerUnit;

        _loseWindow = loseWindow;
        _shopWindow = shopWindow;
        _winWindow = winWindow;

        _playerMover.Moved += OnCatchedPlayerInput;
        _playerShooting.GettedCommandShoot += OnCatchedPlayerInput;

        _shopWindow.Open();
        _resultData = resultData;
        _wallet = wallet;
        _currency = currency;
        _isShootingWith2Hands = isShootingWith2Hands;
    }


    private readonly IUnitsWavesSpawner _spawner;
    private readonly PlayerCharacterMover _playerMover;
    private readonly IPlayerShooting _playerShooting;
    private readonly IWithHp _loseConditionUnit;
    private readonly ShopWindow _shopWindow;
    private readonly LoseWindow _loseWindow;
    private readonly WinWindow _winWindow;
    private readonly IsShootingWith2Hands _isShootingWith2Hands;

    private readonly LevelResultData _resultData;
    private readonly WalletProvider _wallet;
    private readonly CurrencyDescriptor _currency;

    private CancellationTokenSource _cts;
    private bool _isEndedGame;


    protected override void BeginGameImpl()
    {
        _spawner.Dead += OnDeadEnemy;
        _loseConditionUnit.DecreasedHp += OnDecreasedHp;

        _shopWindow.Close();
        _spawner.Spawn();
    }

    private void OnCatchedPlayerInput()
    {
        _playerMover.Moved -= OnCatchedPlayerInput;
        _playerShooting.GettedCommandShoot -= OnCatchedPlayerInput;

        CatchUserInput();
    }

    private async void OnDecreasedHp()
    {
        if (_isEndedGame) return;

        if (_loseConditionUnit.Hp.Current <= 0)
        {
            bool isAviableAds = Application.isEditor ? true : RewardedAds.Instance.IsCanShow();

            _loseWindow.Open(isAviableAds);
            PauseGame();

            LoseWindow.Result result = await _loseWindow.LoseWindowTask;
            if (result == LoseWindow.Result.Resurrect)
            {
                if (Application.isEditor)
                    _loseConditionUnit.Hp.Maximize();
                else
                {
                    if (RewardedAds.Instance.TryShow())
                        await RewardedAds.Instance.ShowingTask;

                    _loseConditionUnit.Hp.Maximize();
                }
               
                ContinueGame();
            }
            else
            {
                _isEndedGame = true;

                // Need return time scale before go to main menu
                ContinueGame();
                LosePlayer();
            }

            _isShootingWith2Hands.Set(false);
            _loseWindow.Close();
        }
    }

    private async void OnDeadEnemy(IWithId unit)
    {
        if (_isEndedGame) return;

        if (_spawner.LivingCount == 0 && _spawner.LeftSpawnCount == 0)
        {
            _isEndedGame = true;
            _cts = new CancellationTokenSource();

            bool isCanceled = await UniTaskHelper.Delay(3f, _cts.Token);
            if (isCanceled) return;

            bool isAviableAds = Application.isEditor ? true : RewardedAds.Instance.IsCanShow();
            _winWindow.Open(isAviableAds);
            PauseGame();

            WinWindow.Result result = await _winWindow.WinWindowTask;
            int award = _resultData.Coins;
            if (result == WinWindow.Result.AdsContinue)
            {
                if (Application.isEditor)
                    award = _resultData.RewardAdsCoins;
                else
                {
                    if (RewardedAds.Instance.TryShow())
                        await RewardedAds.Instance.ShowingTask;

                    award = _resultData.RewardAdsCoins;
                }
            }

            _winWindow.CoinsFlyer.ToFly(award);

            isCanceled = await _winWindow.CoinsFlyer.FirstCoinCompleteTask.SuppressCancellationThrow();
            if (isCanceled) return;

            _wallet.TryApplyDelta(_currency, award);

            isCanceled = await _winWindow.CoinsFlyer.FullCompleteTask.SuppressCancellationThrow();
            if (isCanceled) return;

            
            isCanceled = await UniTaskHelper.Delay(1f, true, _cts.Token);
            if (isCanceled) return;

            _winWindow.Close();
            _isShootingWith2Hands.Set(false);

            // Need return time scale before go to main menu
            ContinueGame();
            WinPlayer();
        }
    }


    public override void Dispose()
    {
        _playerMover.Moved -= OnCatchedPlayerInput;
        _playerShooting.GettedCommandShoot -= OnCatchedPlayerInput;

        _loseConditionUnit.DecreasedHp -= OnDecreasedHp;

        _spawner.Dead -= OnDeadEnemy;

        _cts?.Dispose();
    }
}
