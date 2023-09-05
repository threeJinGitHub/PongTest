using Pong.Settings;
using UnityEngine;
using Zenject;

namespace Pong.Player
{
    public class Board : MonoBehaviour, IBoard
    {
        [SerializeField] private Side _boardSide;

        public Side BoardSide => _boardSide;
        public Vector3 BoardPosition => transform.position;

        public Vector3 BoardSize
        {
            get => transform.localScale;
            set
            {
                transform.localScale = value;
                CheckOutOfBound(transform.position);
            }
        }

        private float _maxSpeed;
        private float _upperBound;

        private float UpperBound
        {
            get => _upperBound - BoardSize.y / 2f;
            set => _upperBound = value;
        }

        [Inject]
        private void Inject(GameSettings gameSettings)
        {
            _maxSpeed = gameSettings.BoardSettings.MaxSpeed;
            UpperBound = gameSettings.GameFiledSettings.UpperWallPositionY;
        }

        private Vector3 CheckOutOfBound(Vector3 position)
        {
            if (Mathf.Abs(position.y) > UpperBound)
            {
                position.y = Mathf.Sign(position.y) * UpperBound;
            }

            return position;
        }


        public void Move(float distance, Direction direction)
        {
            var pos = transform.position;
            if (direction == Direction.Up)
                pos += Mathf.Min(distance, _maxSpeed * Time.fixedDeltaTime) * Vector3.up;
            else
                pos += Mathf.Min(distance, _maxSpeed * Time.fixedDeltaTime) * Vector3.down;

            transform.position = CheckOutOfBound(pos);
        }
    }
}