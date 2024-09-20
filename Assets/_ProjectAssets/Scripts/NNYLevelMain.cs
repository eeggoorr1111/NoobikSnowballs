using Cysharp.Threading.Tasks;
using Narratore;
using Narratore.Components;

public sealed class NNYLevelMain : LevelMain
{
    public NNYLevelMain(LevelConfig config, GameStateEventsHandlers gameStateHandlers, TimeScale timeScale) : base(config, gameStateHandlers, timeScale)
    {
    }


    public override void Dispose()
    {
    }

    public override UniTask OpenMainMenu()
    {
        return new UniTask();
    }
}
