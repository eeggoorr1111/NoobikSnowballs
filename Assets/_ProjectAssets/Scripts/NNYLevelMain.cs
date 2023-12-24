using Narratore;
using Narratore.Abstractions;
using Narratore.Solutions.Battle;

public sealed class NNYLevelMain : LevelMain
{
    public NNYLevelMain(IUnitsWavesSpawner spawner,
                        IUpdatables preparedUpdatables,
                        IUpdatables beginnedUpdatables,
                        LevelConfig config) : base(preparedUpdatables, beginnedUpdatables, config)
    {
        _spawner = spawner;
    }


    private readonly IUnitsWavesSpawner _spawner;


    protected override void BeginGameImpl()
    {
        _spawner.Spawn();
    }

    protected override void UpdateImpl()
    {
    }


    public override void Dispose()
    {

    }
}
