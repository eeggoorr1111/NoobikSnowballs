using Narratore;
using Narratore.Abstractions;

public sealed class NNYLevelMain : LevelMain
{
    public NNYLevelMain(IUpdatables preparedUpdatables,
                        IUpdatables beginnedUpdatables,
                        LevelConfig config) : base(preparedUpdatables, beginnedUpdatables, config)
    {
    }


    public override void Dispose()
    {

    }
}
