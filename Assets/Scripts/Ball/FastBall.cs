using System.Linq;
using UniRx;
using UnityEngine;

namespace Pong.Balls
{
    [RequireComponent(typeof(Collider2D))]
    public class FastBall : BaseBall
    {
        private Vector3 _previousPosition;
        private Transform _previousHitTransform;

        private void Awake() => Observable.EveryFixedUpdate().Subscribe(_ =>
        {
            CheckCollision();
            _previousPosition = Position;
            Position += Direction * Time.fixedDeltaTime * Speed;
        }).AddTo(this);

        private void CheckCollision()
        {
            var hits = Physics2D.LinecastAll(_previousPosition, Position);
            var hit = hits.FirstOrDefault(x =>
                x.transform != null && x.transform != transform && !x.transform.tag.Equals(BallTag));
            if (hit != default && _previousHitTransform != hit.transform)
            {
                transform.position = _previousPosition;
                ChangeDirection(hit.transform.tag, HitPointToLocalNormalizedBoardPositionX(hit.point, hit.transform));
                _previousHitTransform = hit.transform;
            }
            else
            {
                _previousHitTransform = null;
            }
        }

        public override void Init()
        {
            gameObject.SetActive(true);
            _previousPosition = Position;
        }

        public override void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}