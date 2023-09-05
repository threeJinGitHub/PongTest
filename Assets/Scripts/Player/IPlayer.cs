using UniRx;

namespace Pong.Player
{
    public interface IPlayer
    {
        Side Side { get; }
        ReactiveProperty<int> PlayerScore { get; }
        void StartControllingBoard();
        void StopControllingBoard();
    }    
}