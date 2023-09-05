using Pong.Player;
using UniRx;

namespace Pong.Match
{
    public interface IMatchControlService : IDeactivated
    {
        public ReactiveCommand<Side> OnMatchEnd { get; }
    
        void StartMatch();
    
        (IPlayer player1, IPlayer player2) GetPlayersInMatch();
    }    
}
