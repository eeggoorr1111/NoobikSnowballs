using Narratore;
using Narratore.Abstractions;
using Narratore.Analytics;
using Narratore.DI;
using Narratore.Solutions.GamePush;
using System.Collections.Generic;


public class NNY_MainDI : MainDI<DataProviders> 
{
    protected override IReadOnlyList<Sender> CreateAnalytics()
    {
        return new List<Sender>() { new GameAnalyticsSender() };
    }

    protected override FullscreenAds CreateFullscreenAds(bool isDebugAds, IAnalytic analytics)
    {
        if (isDebugAds)
            return new StubFullscreenAds(analytics);
        else
            return new GamePushFullscreenAds(analytics);
    }

    protected override RewardedAds CreateRewardedAds(bool isDebugAds, bool isWithAwardStubRewarded, IAnalytic analytics)
    {
        if (isDebugAds)
            return new StubRewardedAds(isWithAwardStubRewarded, analytics);
        else
            return new GamePushRewardedAds(_audioPlayer, analytics);
    }

    protected override GameReady CreateGameReady(bool isDebugApi, IGameLoop loop)
    {
        if (isDebugApi)
            return new StubGameReady(loop);
        else
            return new GamePushGameReadyAdapter(loop);
    }

    protected override DataLoader CreateDataLoader(DataLoader.Mode mode, DataProviders config) =>
        new NNY_DataLoader(mode, config);
}
