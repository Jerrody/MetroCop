using System.Collections;
using System.Runtime.CompilerServices;
using Cars;
using Road;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    public sealed class PlayerCarController : CarController
    {
        public static PlayerCarController instance { get; private set; }

        [Header("Info")] [SerializeField] private Vector3[] positionsOnRoad;

        [field: Header("Movement")]
        [field: SerializeField]
        public float speed { get; private set; } = 1.0f;

        [SerializeField] private float maxSpeed = 25.0f;
        [SerializeField] private float increaseSpeedRate = 0.1f;

        public float Speed => speed;
        public float HealthPercentage => healthComponent.HealthPercentage;

        public int CurrentPositionIndex { get; private set; }

        private int maxPositionIndex => positionsOnRoad.Length - 1;

        private Transform _transform;


        private void Awake()
        {
            instance = this;

            _transform = transform;

            healthComponent.Prepare();
            healthComponent.DeathEvent += OnDeath;
        }

        private void Start()
        {
            CurrentPositionIndex = Random.Range(default, positionsOnRoad.Length);
            var newPosition = positionsOnRoad[CurrentPositionIndex];
            newPosition.x = _transform.position.x;
            _transform.position = newPosition;

            StartCoroutine(IncreaseSpeed());
        }

        private void Update()
        {
            var playerPosition = _transform.position;
            var newPosition = playerPosition;
            newPosition.x -= 5.0f;

            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }

            _transform.position = Vector3.Lerp(playerPosition, newPosition, speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            /*if (other.gameObject.TryGetComponent<PoliceController>(out var policeCar))
            {
                policeCar.DestroyCar();
            }*/
            if (other.gameObject.TryGetComponent<CanisterController>(out var canister))
            {
                Destroy(canister.gameObject);
                healthComponent.Heal(canister.healAmount);
            }
        }

        public void OnMoveDown(InputAction.CallbackContext ctx)
        {
            if (ctx.started && CurrentPositionIndex != default)
            {
                CurrentPositionIndex--;
                Move();
            }
        }

        public void OnMoveUp(InputAction.CallbackContext ctx)
        {
            if (ctx.started && CurrentPositionIndex != maxPositionIndex)
            {
                CurrentPositionIndex++;
                Move();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Move()
        {
            var playerPosition = _transform.position;
            var nextPosition = positionsOnRoad[CurrentPositionIndex];
            nextPosition.x = playerPosition.x;
            _transform.position = nextPosition;
        }
        
        public void DecreaseSpeed(float amount)
        {
            speed = Mathf.Clamp(speed - amount, 14.0f, maxSpeed);
        }

        private void OnDeath()
        {
            healthComponent.DeathEvent -= OnDeath;

            // TODO: Call UI.
        }

        private IEnumerator IncreaseSpeed()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                speed += increaseSpeedRate;
            }
        }
    }
}