using Pong.Balls;
using UniRx;

namespace Pong.Boosters
{
    public interface IBooster
    {
        ReactiveCommand<IBaseBall> OnActivateBooster { get; }
        BoosterType BoosterType { get; }
        void Destroy();
    }
}