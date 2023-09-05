using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Pong.Balls
{
    public interface IBallSpawnerService
    {
        IEnumerable<IBaseBall> AllSpawnedBalls { get; }
        ReactiveCommand<IBaseBall> OnBallSpawned { get; }
        ReactiveCommand<IBaseBall> OnBallRemoved { get; }

        void SpawnBall(Vector3 position, Vector3 direction, float speed);
        void SpawnBall(Side side);
        void RemoveBall(IBaseBall ball);
    }
}