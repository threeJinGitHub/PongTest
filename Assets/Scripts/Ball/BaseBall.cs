using UniRx;
using UnityEngine;

namespace Pong.Balls
{
    public abstract class BaseBall : MonoBehaviour, ISwitchableBall
    {
        public ReactiveCommand<Side> OnHitSideWall { get; } = new();
        public ReactiveCommand OnPositionChanged { get; } = new();
        public ReactiveCommand<Side> OnHitBoard { get; } = new();
        public float MaxAngleOfReboundFromBoard { get; set; }

        public Vector3 Position
        {
            get => transform.position;
            set
            {
                OnPositionChanged.Execute();
                transform.position = value;
            }
        }

        public float Speed { get; set; }
        public float AdditionToSpeed { get; set; }

        public Vector3 Direction
        {
            get => _direction;
            set => _direction = value;
        }

        private Vector3 _direction;
        protected const string BallTag = "Ball";
        private const string HorizontalWallTag = "wall";
        private const string VerticalWallTag = "side_wall";
        private const string BoardTag = "board";

        protected void ChangeDirection(string collisionObjTag, float boardHitPositionY)
        {
            switch (collisionObjTag)
            {
                case HorizontalWallTag:
                    _direction.y *= -1;
                    break;
                case BoardTag:
                {
                    var verticalDirectionPercent = boardHitPositionY * MaxAngleOfReboundFromBoard / 180f;
                    _direction.x = -Mathf.Sign(_direction.x) * (1 - Mathf.Abs(verticalDirectionPercent));
                    _direction.y = verticalDirectionPercent;
                    _direction = _direction.normalized;
                    Speed += AdditionToSpeed;
                    OnHitBoard.Execute(Position.x > 0 ? Side.Right : Side.Left);
                    break;
                }
                case VerticalWallTag:
                    OnHitSideWall.Execute(Position.x > 0 ? Side.Right : Side.Left);
                    break;
            }
        }

        protected static float HitPointToLocalNormalizedBoardPositionX(Vector2 hitPoint, Transform board)
        {
            var localNormalizedBoardPositionY = (hitPoint - (Vector2) board.position).y / board.localScale.y * 2;
            return localNormalizedBoardPositionY;
        }

        public abstract void Init();

        public abstract void Deactivate();
    }
}