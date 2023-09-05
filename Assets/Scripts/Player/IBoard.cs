using UnityEngine;

namespace Pong.Player
{
    public interface IBoard
    {
        Side BoardSide { get; }
        void Move(float distance, Direction direction);
        Vector3 BoardPosition { get; }
        Vector3 BoardSize { get; set; }
    }
}