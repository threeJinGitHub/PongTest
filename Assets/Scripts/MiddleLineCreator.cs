using UnityEngine;

namespace Pong.Utils
{
    public class MiddleLineCreator : MonoBehaviour
    {
        [SerializeField, Range(1, 99)] private int _dotsCount;
        [Space] [SerializeReference] private Transform _squarePrefab;
        [SerializeReference] private Transform _parent;
    }
}