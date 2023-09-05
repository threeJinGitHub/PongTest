using Pong.Balls;
using UniRx;
using UnityEngine;

namespace Pong.Boosters
{
    public class Booster : MonoBehaviour, IBooster
    {
        [SerializeField] private BoosterType _boosterType;
        public BoosterType BoosterType => _boosterType;
    
        public void Destroy() => Object.Destroy(gameObject);

        public ReactiveCommand<IBaseBall> OnActivateBooster { get; } = new();

        private void OnTriggerEnter2D(Component other)
        {
            if (other.TryGetComponent<BaseBall>(out var ball))
            {
                OnActivateBooster.Execute(ball);
            }
        }
    }
}
