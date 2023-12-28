using Narratore;
using Narratore.Abstractions;
using Narratore.Solutions.Battle;
using Narratore.UI;

public sealed class NNYLevelMain : LevelMain
{
    public NNYLevelMain(IUnitsWavesSpawner spawner,
                        IUpdatables preparedUpdatables,
                        IUpdatables beginnedUpdatables,
                        LevelConfig config,
                        PlayerCharacterMover playerMover,
                        IPlayerShooting playerShooting,
                        ShopWindow shopWindow) : base(preparedUpdatables, beginnedUpdatables, config)
    {
        _spawner = spawner;
        _playerMover = playerMover;
        _playerShooting = playerShooting;
        _shopWindow = shopWindow;

        _playerMover.Moved += OnCatchedPlayerInput;
        _playerShooting.GettedCommandShoot += OnCatchedPlayerInput;

        _shopWindow.Open();
    }


    private readonly IUnitsWavesSpawner _spawner;
    private readonly PlayerCharacterMover _playerMover;
    private readonly IPlayerShooting _playerShooting;
    private readonly ShopWindow _shopWindow;


    protected override void BeginGameImpl()
    {
        _shopWindow.Close();
        _spawner.Spawn();
    }

    private void OnCatchedPlayerInput()
    {
        _playerMover.Moved -= OnCatchedPlayerInput;
        _playerShooting.GettedCommandShoot -= OnCatchedPlayerInput;

        CatchUserInput();
    }

    public override void Dispose()
    {
        _playerMover.Moved -= OnCatchedPlayerInput;
        _playerShooting.GettedCommandShoot -= OnCatchedPlayerInput;
    }
}
