using UniRx;

namespace Pong.Boosters
{
    public interface IBoosterSpawnService : IDeactivated
    {
        ReactiveCommand<IBooster> OnSpawnBooster { get; }
        void StartSpawn();
        void StopSpawn();
    }
}