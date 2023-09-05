namespace Pong.Input
{
    public interface IReversibleInputService : IInputService
    {
        bool IsReverse { get; set; }
    }    
}
