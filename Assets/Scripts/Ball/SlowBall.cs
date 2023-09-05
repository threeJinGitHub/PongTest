using UnityEngine;

namespace Pong.Balls
{
    public class SlowBall : BaseBall
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform != null && other.transform.tag.Equals(BallTag))
            {
                ChangeDirection(other.transform.tag,
                    HitPointToLocalNormalizedBoardPositionX(other.contacts[0].point, other.transform));
            }
        }

        private void FixedUpdate() => Position += Direction * Time.fixedDeltaTime * Speed;

        public override void Init() => gameObject.SetActive(true);

        public override void Deactivate() => gameObject.SetActive(false);
    }
}
