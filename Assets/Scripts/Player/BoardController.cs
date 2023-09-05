using Pong.Input;
using UniRx;

namespace Pong.Player
{
    public class BoardController : IBoardController
    {
        public IBoard Board { get; }
        public IReversibleInputService Input { get; }
        private readonly CompositeDisposable _disposables = new();

        public BoardController(IBoard board, IReversibleInputService input)
        {
            Board = board;
            Input = input;
        }

        public void Deactivate()
        {
            _disposables?.Clear();
        }

        public void Init()
        {
            Input.OnMoveDown.Subscribe(distance => Board.Move(distance, Direction.Down)).AddTo(_disposables);
            Input.OnMoveUp.Subscribe(distance => Board.Move(distance, Direction.Up)).AddTo(_disposables);
        }
    }
}