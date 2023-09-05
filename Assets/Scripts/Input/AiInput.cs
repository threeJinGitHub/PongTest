using UniRx;

namespace Pong.Input
{
    public class AiInput : IReversibleInputService
    {
        public ReactiveCommand<float> OnMoveUp { get; } = new();
        public ReactiveCommand<float> OnMoveDown { get; } = new();
        public bool IsReverse { get; set; }
    }
}