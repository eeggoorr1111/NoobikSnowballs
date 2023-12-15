using Narratore;
using Narratore.Abstractions;

public sealed class NNYLevelMain : LevelMain
{
    public NNYLevelMain(IPreparedUpdatables preparedUpdatables,
                        IBeginnedUpdatables beginnedUpdatables,
                        LevelConfig config,
                        NNYLevelProgress progress) : base(preparedUpdatables, beginnedUpdatables, config)
    {
        _progress = progress;

        UnityEngine.Debug.LogError("KEK");
    }

    public override Progress Progress => _progress;


    private readonly NNYLevelProgress _progress;
}
