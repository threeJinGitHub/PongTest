using UniRx;
using UnityEngine;

namespace Pong.Balls
{
    public interface IBaseBall
    {
        Vector3 Direction { get; set; }
        float Speed { get; set; }
        float MaxAngleOfReboundFromBoard { get; set; }
        float AdditionToSpeed { get; set; }
        Vector3 Position { get; set; }
        ReactiveCommand OnPositionChanged { get; }
        ReactiveCommand<Side> OnHitSideWall { get; }
        ReactiveCommand<Side> OnHitBoard { get; }
    }
}