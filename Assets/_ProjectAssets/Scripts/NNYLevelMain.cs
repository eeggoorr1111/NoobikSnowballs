using Narratore;
using Narratore.Abstractions;

public sealed class NNYLevelMain : LevelMain
{
    public NNYLevelMain(IUpdatables preparedUpdatables,
                        IUpdatables beginnedUpdatables,
                        LevelConfig config) : base(preparedUpdatables, beginnedUpdatables, config)
    {
        UnityEngine.Debug.LogError($"{preparedUpdatables == null} {beginnedUpdatables == null}");
    }


    public override void Dispose()
    {

    }
}
