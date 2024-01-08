﻿using Narratore.Pools;
using Narratore.Solutions.Battle;
using Narratore.UI;
using Narratore.UnityUpdate;
using Narratore.WorkWithMesh;
using TMPro;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Narratore.DI
{


    public class NNYLevelAreaConfigurator : Configurator
    {
        [SerializeField] private MeshFrame _area;
        [SerializeField] private LoopedTextFadeAnimation _warning;
        [SerializeField] private LevelAreaConfig _config;

        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            builder.Register<LevelAreaHandler>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces()
                .WithParameter(_area)
                .WithParameter(_config)
                .WithParameter(_warning);
        }
    }
}

