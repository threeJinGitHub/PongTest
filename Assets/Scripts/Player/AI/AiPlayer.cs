using System;
using System.Collections.Generic;
using System.Linq;
using Pong.Balls;
using Pong.Settings;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pong.Player.Ai
{
    public class AiPlayer : Player
    {
        private readonly List<IBaseBall> _balls = new();
        private Vector3 _previousDirection;
        private float _heightBoardShift;
        private float _dist;
        private readonly float _heightLimit;
        private readonly CompositeDisposable _disposables = new();
        private readonly IBallSpawnerService _ballSpawnerService;
        private const float MaxDist = 15f;
        private bool _previousDirectionIsDown;

        #region AiSettings

        private readonly float _dragSpeed;
        private readonly float _boardPositionAccuracy;
        private readonly float _randomness;
        private readonly int _wallCollisionCheck;

        #endregion

        private Vector3 BoardPosition
        {
            get
            {
                var pos = Board.BoardPosition;
                pos.y += _heightBoardShift;
                pos.x += (Math.Sign(Board.BoardPosition.x) > 0 ? .5f : -.5f) * Board.BoardSize.x;
                return pos;
            }
        }

        public AiPlayer(IBoardController boardController, IBallSpawnerService ballSpawnerService,
            GameSettings gameSettings)
            : base(boardController)
        {
            (_wallCollisionCheck, _boardPositionAccuracy, _dragSpeed, _randomness) =
                gameSettings.AiSettings.GetAIParams();
            _heightLimit = gameSettings.GameFiledSettings.UpperWallPositionY;
            _ballSpawnerService = ballSpawnerService;
        }

        public override void StopControllingBoard()
        {
            base.StopControllingBoard();
            _balls.Clear();
            _disposables?.Clear();
        }

        private void AddBall(IBaseBall ball)
        {
            ball.OnPositionChanged.Subscribe(_ =>
            {
                if (GetNearestBall() == ball)
                {
                    OnBallPositionUpdated(ball);
                }
            }).AddTo(_disposables);
            _balls.Add(ball);
        }

        public override void StartControllingBoard()
        {
            base.StartControllingBoard();
            _ballSpawnerService.AllSpawnedBalls.ToList().ForEach(AddBall);
            _ballSpawnerService.OnBallSpawned.Subscribe(AddBall).AddTo(_disposables);
            _ballSpawnerService.OnBallRemoved.Subscribe(x => _balls.Remove(x)).AddTo(_disposables);
        }

        private IBaseBall GetNearestBall()
        {
            IBaseBall nearestBall = null;
            var minDist = 1e9f;
            foreach (var ball in _balls)
            {
                var dist = (ball.Position - BoardPosition).magnitude;
                if (dist > minDist) continue;
                minDist = dist;
                nearestBall = ball;
            }

            return nearestBall;
        }

        private void OnBallPositionUpdated(IBaseBall ball)
        {
            if (Math.Sign(_previousDirection.x) != Math.Sign(ball.Direction.x))
            {
                OnDirectionChanged();
            }

            if (Math.Sign(ball.Direction.x) == Math.Sign(BoardPosition.x))
            {
                CalculateNewInputData(ball.Direction, ball.Position);
            }
            else
            {
                MoveToCenter();
            }

            _previousDirection = ball.Direction;
        }

        private void OnDirectionChanged() =>
            _heightBoardShift = Random.Range(-_boardPositionAccuracy, _boardPositionAccuracy);

        private void MoveToCenter() =>
            MoveUpOrDown(BoardPosition.y > 0, Mathf.Abs(BoardPosition.y) * Time.fixedDeltaTime);

        private void MoveUpOrDown(bool down, float distance)
        {
            _previousDirectionIsDown = down;
            if (down)
            {
                Input.OnMoveDown.Execute(distance);
            }
            else
            {
                Input.OnMoveUp.Execute(distance);
            }
        }

        private void CalculateNewInputData(Vector3 ballDirection, Vector3 ballPosition)
        {
            var predictionBallPositionY = CollisionPointPredictionY(ballDirection, ballPosition, _wallCollisionCheck);
            _dist += (predictionBallPositionY - BoardPosition.y) * _dragSpeed * Time.fixedDeltaTime;
            var directionIsDown = BoardPosition.y > predictionBallPositionY;
            if (_previousDirectionIsDown != directionIsDown)
            {
                _dist /= 2;
            }

            if (Mathf.Abs(_dist) > MaxDist)
            {
                _dist = MaxDist * Math.Sign(_dist);
            }

            _previousDirectionIsDown = directionIsDown;
            MoveUpOrDown(directionIsDown, Mathf.Abs(_dist));
        }

        private float CollisionPointPredictionY(Vector3 direction, Vector3 ballPosition, int wallsCollisionCounter)
        {
            Vector3 wallCollision;
            if (Mathf.Abs(direction.y) <= float.Epsilon)
            {
                wallCollision = new Vector3(BoardPosition.x, ballPosition.y);
            }
            else
            {
                wallCollision = ballPosition + direction *
                    Mathf.Abs((Mathf.Sign(direction.y) * _heightLimit - ballPosition.y) / Mathf.Abs(direction.y));
            }

            if (Mathf.Abs(wallCollision.x) > Mathf.Abs(BoardPosition.x) || wallsCollisionCounter == 0)
            {
                wallCollision *= Random.Range(-_randomness, _randomness) + 1;
                return Vector3.Lerp(ballPosition, wallCollision,
                    (ballPosition - BoardPosition).magnitude / (ballPosition - wallCollision).magnitude).y;
            }

            direction.y *= -1;
            return CollisionPointPredictionY(direction, wallCollision, --wallsCollisionCounter);
        }
    }
}