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
                var position = _transform.position;
                var newPosition = position;
                newPosition.x = playerPosition.x;

                position = Vector3.Lerp(position,
                    newPosition,
                    (_playerCar.Speed + 1.0f) * Time.deltaTime);
                _transform.position = position;

                yield return null;
            }
        }

        public void DestroyCar()
        {
            Destroy(gameObject);
            PoliceManager.DestroyPoliceEvent.Invoke();
        }
    }
}