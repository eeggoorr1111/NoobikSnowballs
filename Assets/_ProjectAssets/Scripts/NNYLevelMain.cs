using Narratore;
using Narratore.Abstractions;
using Narratore.Solutions.Battle;

public sealed class NNYLevelMain : LevelMain
{
    public NNYLevelMain(IUnitsSpawner<CreeperRoster> spawner,
                        IHeldPoints points,
                        IUpdatables preparedUpdatables,
                        IUpdatables beginnedUpdatables,
                        LevelConfig config) : base(preparedUpdatables, beginnedUpdatables, config)
    {
        _spawnPoints = points;
        _spawner = spawner;
    }


    private readonly IHeldPoints _spawnPoints;
    private readonly IUnitsSpawner<CreeperRoster> _spawner;


    protected override void UpdateImpl()
    {
        _spawner.Spawn(PlayersIds.GetBotId(1), _spawnPoints.Get());
    }


    public override void Dispose()
    {

    }
}
