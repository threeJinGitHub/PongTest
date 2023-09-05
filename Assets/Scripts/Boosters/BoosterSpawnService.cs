using Cysharp.Threading.Tasks;
using Pong.Settings;
using UniRx;
using UnityEngine;

namespace Pong.Boosters
{
    public class BoosterSpawnService : IBoosterSpawnService
    {
        public ReactiveCommand<IBooster> OnSpawnBooster { get; } = new();

        private readonly float _spawnInterval;
        private readonly IBoosterFactory<IBooster> _boosterFactory;
        private readonly Vector2 _boarders;
        private bool _spawn;

        public BoosterSpawnService(BoosterFactory boosterFactory, GameSettings gameSettings)
        {
            _spawnInterval = gameSettings.BoostersSetting.SpawnInterval;
            _boosterFactory = boosterFactory;
            _boarders = new Vector2(gameSettings.GameFiledSettings.RightWallPositionX,
                gameSettings.GameFiledSettings.UpperWallPositionY);
        }

        public async void StartSpawn()
        {
            _spawn = true;
            while (_spawn)
            {
                await UniTask.WaitForSeconds(_spawnInterval);
                if (!_spawn) return;
                var spawnPosition = new Vector2(Random.Range(-_boarders.x, _boarders.x) * .7f,
                    Random.Range(-_boarders.y, _boarders.y) * .9f);
                OnSpawnBooster?.Execute(_boosterFactory.GetBooster(spawnPosition));
            }
        }

        public void StopSpawn() => _spawn = false;

        public void Deactivate() => StopSpawn();
    }
}