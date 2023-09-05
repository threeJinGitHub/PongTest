using Pong.Input;

namespace Pong.Player
{
    public interface IBoardController : IDeactivated, IInitializable
    {
        IBoard Board { get; }
        IReversibleInputService Input { get; }
    }
}