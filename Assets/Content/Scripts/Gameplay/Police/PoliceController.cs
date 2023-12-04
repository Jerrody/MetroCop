using System;
using System.Collections;
using Player;
using UnityEngine;

namespace Cars
{
    public sealed class PoliceController : MonoBehaviour
    {
        private Transform _transform;
        private PlayerCarController _playerCar;

        private bool _isAtPositionWithPlayer;

        private void Awake()
        {
            _transform = transform;
        }

        private void Start()
        {
            _playerCar = PlayerCarController.instance;

            StartCoroutine(MoveToPosition());
        }

        private void Update()
        {
            var playerPosition = _playerCar.transform.position;
            var newPosition = _transform.position;
            newPosition.x = playerPosition.x;

            _transform.position = newPosition;
        }

        private IEnumerator MoveToPosition()
        {
            while (!_isAtPositionWithPlayer)
            {
                var playerPosition = _playerCar.transform.position;
                var newPosition = _transform.position;
                newPosition.x = playerPosition.x;

                _transform.position = Vector3.Lerp(_transform.position,
                    newPosition,
                    (_playerCar.Speed + 1.0f) * Time.deltaTime);

                yield return null;
            }
        }
    }
}