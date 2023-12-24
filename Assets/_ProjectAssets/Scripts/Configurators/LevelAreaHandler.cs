using Narratore.Extensions;
using Narratore.Helpers;
using Narratore.Interfaces;
using Narratore.Solutions.Battle;
using Narratore.UI;
using Narratore.WorkWithMesh;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Narratore.DI
{
    public class LevelAreaHandler : IUpdatable
    {
        public LevelAreaHandler(MeshFrame area, 
                                EntityCount enemies, 
                                IPlayerUnitRoot playerUnit,
                                LoopedTextFadeAnimation warning, 
                                float spawnDelay, 
                                IReadOnlyList<IUnitsSpawner> spawners, 
                                IHeldPoints spawnPoints)
        {
            _area = area;
            _enemies = enemies;
            _playerUnit = playerUnit;
            _warning = warning;
            _spawnDelay = spawnDelay;
            _spawners = spawners;
            _spawnPoints = spawnPoints;

            Vector3 center = _area.transform.position;

            _levelAreaBounds = new Bounds(center, _area.Size.To3D(Enums.TwoAxis.XZ, 10f));
        }


        private readonly MeshFrame _area;
        private readonly EntityCount _enemies;
        private readonly IPlayerUnitRoot _playerUnit;
        private readonly Bounds _levelAreaBounds;
        private readonly LoopedTextFadeAnimation _warning;
        private readonly float _spawnDelay;
        private readonly IReadOnlyList<IUnitsSpawner> _spawners;
        private readonly IHeldPoints _spawnPoints;

        private CancellationTokenSource _spawning;
        private bool _isSpawned;
        

        public void Tick()
        {
            if (_isSpawned && _spawning == null) return;

            if (_levelAreaBounds.Contains(_playerUnit.Root.position))
            {
                if (_spawning != null)
                {
                    _warning.Disable();

                    _spawning.Cancel();
                    _spawning = null;
                }
            }
            else if (_spawning == null)
                TrySpawn();
        }


        private async void TrySpawn()
        {
            _spawning = new CancellationTokenSource();
            _warning.Enable();

            bool isCanceled = await UniTaskHelper.Delay(_spawnDelay, _spawning.Token);
            if (isCanceled) return;

            if (TryGetSpawner(out IUnitsSpawner spawner))
            {
                for (int i = 0; i < _enemies.Item2; i++)
                    spawner.Spawn(PlayersIds.GetBotId(2), _spawnPoints.Get());
            }
            else
                Debug.LogWarning($"In level area not found spawner for unit {_enemies.Item1.name}");

            _isSpawned = true;
        }

        private bool TryGetSpawner(out IUnitsSpawner spawner)
        {
            spawner = null;
            foreach (var check in _spawners)
                if (check.Sample == _enemies.Item1)
                {
                    spawner = check;
                    return true;
                }

            return false;
        }
    }
}

