using UniRx;
using UnityEngine;

namespace Pong.Input
{
    public class BasePlayerInput : MonoBehaviour, IReversibleInputService
    {
        public float Sensitivity { get; set; }
        public ReactiveCommand<float> OnMoveUp { get; } = new();
        public ReactiveCommand<float> OnMoveDown { get; } = new();
        public bool IsReverse { get; set; }
    }
}