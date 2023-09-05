using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Pong.Balls
{
    public class BallPoolFactory : MonoBehaviour
    {
        [SerializeField] private BaseBall _baseBallPrefab;
        private DiContainer _container;
        public List<ISwitchableBall> Balls { get; } = new();
        private readonly List<ISwitchableBall> _ballsPool = new();

        [Inject]
        private void Inject(DiContainer container)
        {
            _container = container;
        }

        public ISwitchableBall GetBall(Vector3 position, Vector3 direction, float speed, float additionToSpeed,
            float maxAngleOfReboundFromBoard)
        {
            ISwitchableBall ball;
            if (_ballsPool.Count == 0)
            {
                ball = _container.InstantiatePrefab(_baseBallPrefab).GetComponent<BaseBall>();
            }
            else
            {
                _ballsPool.Remove(ball = _ballsPool[^1]);
            }

            ball.AdditionToSpeed = additionToSpeed;
            ball.Position = position; 
            ball.Direction = direction;
            ball.Speed = speed;
            ball.MaxAngleOfReboundFromBoard = maxAngleOfReboundFromBoard;
            ball.Init();
            Balls.Add(ball);
            return ball;
        }

        public void TryDeactivateBall(IBaseBall ball)
        {
            if (Balls.All(x => x != ball)) return;
            var switchableBall = Balls.First(x => x == ball);
            switchableBall.Deactivate();
            Balls.Remove(switchableBall);
            _ballsPool.Add(switchableBall);
        }
    }
}