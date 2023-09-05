using Pong.Input;
using UniRx;

namespace Pong.Player
{
    public class Player : IPlayer
    {
        protected readonly IBoard Board;
        protected readonly IReversibleInputService Input;
        private readonly IBoardController _boardController;

        public ReactiveProperty<int> PlayerScore { get; set; } = new();
        public Side Side => Board.BoardSide;

        public Player(IBoardController boardController)
        {
            _boardController = boardController;
            Board = boardController.Board;
            Input = boardController.Input;
        }

        public virtual void StartControllingBoard() => _boardController.Init();

        public virtual void StopControllingBoard() => _boardController.Deactivate();
    }
}