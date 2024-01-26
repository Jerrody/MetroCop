using System.Linq;
using Player;
using UnityEngine;

namespace Game.Platforms
{
    public sealed class PlatformManager : MonoBehaviour
    {
        private float _platformZAxisLength;
        private int _currentLastPlatform;

        private RoadController[] _roads;
        private PlayerCarController _playerCar;
        
        private void Awake()
        {
            _playerCar = FindFirstObjectByType<PlayerCarController>();
            _roads = FindObjectsOfType<RoadController>(true);
            _roads = _roads.OrderByDescending(x => x.transform.position.x).ToArray();
            _platformZAxisLength = _roads.First().length;
        }

        private void Update()
        {
            if (Vector3.Distance(_playerCar.transform.position, _roads[_currentLastPlatform].transform.position) >
                _platformZAxisLength + 20.0f)
            {
                MoveForwardLast(_currentLastPlatform);
                _currentLastPlatform = (_currentLastPlatform + 1) % _roads.Length;
            }
        }

        private void MoveForwardLast(int platformIndex)
        {
            var roadPosition = _roads[platformIndex].transform.position;
            roadPosition.x -= _platformZAxisLength * 3;
            _roads[platformIndex].MoveForward(roadPosition);
        }
    }
}