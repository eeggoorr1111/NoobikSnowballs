using Narratore.AI;
using Narratore.Solutions.Battle;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VContainer;

namespace Narratore.DI
{
    public class NNYEnemiesConfigurator : EntitiesConfigurator<NNY_BattleData>
    {
        [Header("RECORD MODE")]
        [SerializeField] private LevelModeDescriptor _recordLevelModeKey;
        [SerializeField] private SpawnCurveWavesConfig _recordSpawnWaves;

        [Header("VIEW")]
        [SerializeField] private TMP_Text _enemiesCount;


        [Header("ENEMIES")]
        [SerializeField] private CreeperPoolConfig[] _configs;
        [SerializeField] private BossCreeperPoolConfig _bossConfig;
        [SerializeField] private float _creeperExplosionDistance;


        protected override IReadOnlyList<ISpawner> CreateSpawners(NNY_BattleData data, EntitiesConfiguratorConfig config)
        {
            LevelSpawnWavesConfig[] levels = GetComponentsInChildren<LevelSpawnWavesConfig>();
            LoopedCounter counter = new LoopedCounter(0, levels.Length - 1, config.LvlConfig.RawLevel);
            IReadOnlyList<SpawnWavesConfig> waves = levels[counter.Current].Waves;
            List<ISpawner> spawners = new List<ISpawner>();

            config.Builder.Register<BotsShooting>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(PlayersIds.GetBotId(1));
            config.Builder.Register<SeekSteering>(Lifetime.Singleton).WithParameter(0.0001f);
            config.Builder.Register<EnemiesMover>(Lifetime.Singleton).AsImplementedInterfaces();


            CreeperBattleRegistrator creeperReg = new CreeperBattleRegistrator(data, config.Registrators);
            for (int i = 0; i < _configs.Length; i++)
                spawners.Add(PoolSpawner(creeperReg, data, _configs[i], PlayersIds.GetBotId(1)));

            BossCreeperBattleRegistrator bossReg = new BossCreeperBattleRegistrator(data, config.Registrators);
            spawners.Add(PoolSpawner(bossReg, data, _bossConfig, PlayersIds.GetBotId(1)));

            return spawners;
        }
    }
}

