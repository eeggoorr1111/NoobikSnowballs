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
        spawner.Spawn(PlayersIds.GetBotId(1), points.Get());
    }


    public override void Dispose()
    {

    }
}
