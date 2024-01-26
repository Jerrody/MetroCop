using Player;
using UnityEngine;

namespace Gameplay
{
    public sealed class TerrainController : MonoBehaviour
    {
        private PlayerCarController _playerCar;
        private Terrain _terrain;
        
        private void Awake()
        {
            _playerCar = FindObjectOfType<PlayerCarController>();

            _terrain = GetComponent<Terrain>();
        }

        private void Update()
        {
            var transformItself = transform;
            var position = transformItself.position;
            var playerPosition = _playerCar.transform.position;
            position.x = playerPosition.x - _terrain.terrainData.size.x / 2.0f;

            transformItself.position = position;
        }
    }
}