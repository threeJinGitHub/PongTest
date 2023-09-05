using UnityEngine;

namespace Pong.Boosters
{
    public interface IBoosterFactory<out T>
    {
        T GetBooster(Vector3 at);

        T GetBooster(BoosterType boosterType, Vector3 at);
    }
}