using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pong.Boosters
{
    public class BoosterFactory : MonoBehaviour, IBoosterFactory<IBooster>
    {
        [SerializeField] private List<Booster> _boostersPrefabs;

        public IBooster GetBooster(Vector3 at)
        {
            return Instantiate(_boostersPrefabs[Random.Range(0, _boostersPrefabs.Count)], at, quaternion.identity);
        }

        public IBooster GetBooster(BoosterType boosterType, Vector3 at)
        {
            var booster = _boostersPrefabs.FirstOrDefault(x => x.BoosterType == boosterType);
            if (booster != default)
            {
                return Instantiate(booster, at, quaternion.identity);
            }

            throw new KeyNotFoundException("Wrong boosterType");
        }
    }
}