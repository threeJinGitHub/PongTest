using System.Collections.Generic;
using System.Linq;
using Pong.Balls;
using Pong.Player;
using Pong.Settings;
using UniRx;
using UnityEngine;

namespace Pong.Boosters
{
    public class BoosterService : IDeactivated
    {
        private readonly IBoardController _boardController1;
        private readonly IBoardController _boardController2;
        private readonly List<(IBaseBall ball, IBoardController boardController)> _ballsBoardPair = new();
        private readonly IBallSpawnerService _ballSpawnerService;
        private readonly CompositeDisposable _disposable = new();
        private readonly float _boardIncreaseSizeValue;
        private readonly float _boardDecreaseSizeValue;
        private readonly Dictionary<IBaseBall, CompositeDisposable> _ballHitsEventDisposables = new();
        private readonly Dictionary<IBooster, CompositeDisposable> _activateBoosterEventDisposables = new();


        public BoosterService(IBoardController boardController1,
            IBoardController boardController2,
            BoostersSetting boostersSetting,
            IBallSpawnerService ballSpawnerService,
            IBoosterSpawnService boosterSpawnService)
        {
            _boardIncreaseSizeValue = boostersSetting.BoardIncreaseSizeValue;
            _boardDecreaseSizeValue = boostersSetting.BoardDecreaseSizeValue;

            _boardController1 = boardController1;
            _boardController2 = boardController2;

            (_ballSpawnerService = ballSpawnerService).OnBallSpawned.Subscribe(AddBall).AddTo(_disposable);
            _ballSpawnerService.OnBallRemoved.Subscribe(OnRemoveBall).AddTo(_disposable);

            boosterSpawnService.OnSpawnBooster.Subscribe(AddBooster).AddTo(_disposable);
        }

        private void AddBooster(IBooster booster)
        {
            if (_activateBoosterEventDisposables.ContainsKey(booster))
            {
                _activateBoosterEventDisposables[booster] = new CompositeDisposable();
            }
            else
            {
                _activateBoosterEventDisposables.Add(booster, new CompositeDisposable());
            }

            booster.OnActivateBooster.Subscribe(x => ActivateBooster(booster, x))
                .AddTo(_activateBoosterEventDisposables[booster]);
        }

        private void RemoveBooster(IBooster booster)
        {
            if (!_activateBoosterEventDisposables.ContainsKey(booster)) return;
            _activateBoosterEventDisposables[booster]?.Clear();
            _activateBoosterEventDisposables.Remove(booster);
        }

        private void AddBall(IBaseBall ball)
        {
            if (_ballsBoardPair.Any(x => x.ball == ball)) return;

            _ballsBoardPair.Add((ball, null));
            if (_ballHitsEventDisposables.ContainsKey(ball))
            {
                _ballHitsEventDisposables[ball] = new CompositeDisposable();
            }
            else
            {
                _ballHitsEventDisposables.Add(ball, new CompositeDisposable());
            }

            ball.OnHitBoard.Subscribe(side =>
            {
                for (var i = 0; i < _ballsBoardPair.Count; i++)
                {
                    if (_ballsBoardPair[i].ball != ball) continue;
                    _ballsBoardPair[i] = (ball,
                        side == _boardController1.Board.BoardSide ? _boardController1 : _boardController2);
                    break;
                }
            }).AddTo(_ballHitsEventDisposables[ball]);
        }

        private void OnRemoveBall(IBaseBall ball)
        {
            if (!_ballHitsEventDisposables.ContainsKey(ball)) return;
            _ballHitsEventDisposables[ball]?.Clear();
            _ballHitsEventDisposables.Remove(ball);
            _ballsBoardPair.Remove(_ballsBoardPair.FirstOrDefault(x => x.ball == ball));
        }

        private void ActivateBooster(IBooster booster, IBaseBall ball)
        {
            var boardController = _ballsBoardPair.First(x => x.ball == ball).boardController;

            if (boardController == null) return;
            switch (booster.BoosterType)
            {
                case BoosterType.BoardIncreaseSizer:
                {
                    var size = boardController.Board.BoardSize;
                    size.y *= _boardIncreaseSizeValue;
                    boardController.Board.BoardSize = size;
                    break;
                }
                case BoosterType.BoardDecreaseSizer:
                {
                    var size = boardController.Board.BoardSize;
                    size.y *= _boardDecreaseSizeValue;
                    boardController.Board.BoardSize = size;
                    break;
                }
                case BoosterType.DoubleBall:
                    var dir1 = ball.Direction;
                    var dir2 = new Vector2(dir1.x * 2, dir1.y).normalized;
                    dir1 = new Vector2(dir1.x, dir1.y * 2).normalized;
                    _ballSpawnerService.SpawnBall(ball.Position, dir2, ball.Speed);
                    ball.Direction = dir1;
                    break;
                case BoosterType.InputReverse:
                    boardController.Input.IsReverse =
                        !boardController.Input.IsReverse;
                    break;
            }

            RemoveBooster(booster);
            booster.Destroy();
        }

        public void Deactivate()
        {
            foreach (var disposable in _activateBoosterEventDisposables)
            {
                disposable.Value?.Clear();
            }

            foreach (var disposable in _ballHitsEventDisposables)
            {
                disposable.Value?.Clear();
            }

            _activateBoosterEventDisposables.Clear();
            _disposable?.Clear();
        }
    }
}