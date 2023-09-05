using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Pong.Balls;
using Pong.Boosters;
using Pong.Player;
using Pong.Settings;
using UniRx;
using Random = UnityEngine.Random;

namespace Pong.Match
{
    public class MatchControlService : IMatchControlService
    {
        public ReactiveCommand<Side> OnMatchEnd { get; } = new();

        private readonly int _pointToWin;
        private readonly CompositeDisposable _spawnDisposable = new();
        private readonly CompositeDisposable _hitDisposable = new();
        private readonly IBallSpawnerService _ballSpawnerService;
        private readonly IPlayer _player1;
        private readonly IPlayer _player2;
        private readonly List<IBaseBall> _balls = new();
        private readonly IBoosterSpawnService _boosterSpawnService;
        private CancellationTokenSource _cancellationTokenSource;

        private int _matchTimer;
        private bool _matchIsEnded;
        private Side _lastSpawnDirection;

        public MatchControlService(
            IPlayer player1,
            IPlayer player2,
            IBallSpawnerService ballSpawnerService,
            MatchSettings matchSettings,
            IBoosterSpawnService boosterSpawnService)
        {
            _pointToWin = matchSettings.PointsToWin;
            _ballSpawnerService = ballSpawnerService;
            _player1 = player1;
            _player2 = player2;
            _matchTimer = matchSettings.MatchDurationInSec;
            _boosterSpawnService = boosterSpawnService;
        }

        private async void Timer()
        {
            while (_matchTimer-- >= 0)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                await UniTask.WaitForSeconds(1, cancellationToken: _cancellationTokenSource.Token);
                _cancellationTokenSource = null;
                if (_matchIsEnded) return;
            }

            EndMatch();
        }

        private void EndMatch()
        {
            _spawnDisposable?.Clear();
            RemoveBalls();
            _player1.StopControllingBoard();
            _player2.StopControllingBoard();
            OnMatchEnd.Execute(_player1.PlayerScore.Value > _player2.PlayerScore.Value ? _player1.Side :
                _player1.PlayerScore.Value < _player2.PlayerScore.Value ? _player2.Side : Side.None);
            _boosterSpawnService.StopSpawn();
            _matchIsEnded = true;
        }

        public void StartMatch()
        {
            _matchIsEnded = false;
            _lastSpawnDirection = Random.value > .5f ? Side.Left : Side.Right;
            _player1.StartControllingBoard();
            _player2.StartControllingBoard();
            _boosterSpawnService.StartSpawn();
            _ballSpawnerService.OnBallSpawned
                .Subscribe(ball =>
                {
                    _balls.Add(ball);
                    ball.OnHitSideWall.Subscribe(UpdateScore).AddTo(_hitDisposable);
                })
                .AddTo(_spawnDisposable);
            _ballSpawnerService.SpawnBall(_lastSpawnDirection);
            Timer();
        }

        public (IPlayer player1, IPlayer player2) GetPlayersInMatch() => (_player1, _player2);

        private void UpdateScore(Side losingSide)
        {
            if (losingSide == _player1.Side)
            {
                _player2.PlayerScore.Value++;
            }
            else
            {
                _player1.PlayerScore.Value++;
            }

            if (_player1.PlayerScore.Value == _pointToWin || _player2.PlayerScore.Value == _pointToWin)
            {
                EndMatch();
            }
            else
            {
                ContinueMatch();
            }
        }

        private void RemoveBalls()
        {
            _hitDisposable?.Clear();
            _balls.ForEach(ball => _ballSpawnerService.RemoveBall(ball));
            _balls.Clear();
        }

        private void ContinueMatch()
        {
            RemoveBalls();
            _lastSpawnDirection = _lastSpawnDirection == Side.Left ? Side.Right : Side.Left;
            _ballSpawnerService.SpawnBall(_lastSpawnDirection);
        }

        public void Deactivate()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }

            _spawnDisposable?.Clear();
            _hitDisposable?.Clear();
        }
    }
}