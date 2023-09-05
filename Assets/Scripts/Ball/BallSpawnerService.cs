using System.Collections.Generic;
using System.Linq;
using Pong.Settings;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pong.Balls
{
    public class BallSpawnerService : IBallSpawnerService
    {
        public ReactiveCommand<IBaseBall> OnBallSpawned { get; } = new();
        public ReactiveCommand<IBaseBall> OnBallRemoved { get; } = new();

        private const float MaxCreationHeightRatio = .7f;
        private readonly float _initialBallSpeed;
        private readonly float _maxAngleOfReboundOfBallFromBoard;
        private readonly float _upperWallPositionY;
        private readonly float _additionToSpeed;
        private readonly BallPoolFactory _ballFactory;
        public IEnumerable<IBaseBall> AllSpawnedBalls => _ballFactory.Balls.Select(x => (IBaseBall) x).ToList();
    
        public BallSpawnerService(GameSettings gameSettings, BallPoolFactory ballFactory)
        {
            _ballFactory = ballFactory;
            _maxAngleOfReboundOfBallFromBoard = gameSettings.BallSettings.MaxAngleOfReboundFromBoard;
            _initialBallSpeed = gameSettings.BallSettings.InitialSpeed;
            _upperWallPositionY = gameSettings.GameFiledSettings.UpperWallPositionY;
            _additionToSpeed = gameSettings.BallSettings.AdditionToSpeed;
        }

        public void SpawnBall(Vector3 position, Vector3 direction, float speed)
        {
            var ball = _ballFactory.GetBall(position, direction,
                speed, _additionToSpeed, _maxAngleOfReboundOfBallFromBoard);
            OnBallSpawned?.Execute(ball);
        }

        public void SpawnBall(Side side)
        {
            var creationPos = Random.Range(-_upperWallPositionY, _upperWallPositionY) 
                              * MaxCreationHeightRatio * Vector3.up;
            var ballDirection = new Vector3(side == Side.Left ? -1f : 1f, Random.Range(-1f, 1f)).normalized;
            SpawnBall(creationPos, ballDirection, _initialBallSpeed);
        }

        public void RemoveBall(IBaseBall ball)
        {
            OnBallRemoved.Execute(ball);
            _ballFactory.TryDeactivateBall(ball);
        }
    }
}