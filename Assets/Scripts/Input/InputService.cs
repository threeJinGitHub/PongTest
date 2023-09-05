using UniRx;

namespace Pong.Input
{
    public interface IInputService
    {
        public ReactiveCommand<float> OnMoveUp { get; }
        public ReactiveCommand<float> OnMoveDown { get; }
    }    
}