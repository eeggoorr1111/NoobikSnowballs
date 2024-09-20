using Narratore;
using Narratore.DI;


public class NNY_LevelDI : VContainerLevelDI<NNYLevelMain, DataProviders>
{
    protected override DataLoader CreateDataLoader(DataProviders providers) => 
        new NNY_DataLoader(DataLoader.Mode.Editor, providers);
}