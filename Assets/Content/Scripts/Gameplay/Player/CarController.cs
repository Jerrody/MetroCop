using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Player
{
    [Serializable]
    public struct HealthComponent
    {
        public event Action DeathEvent;

        [Header("Info")] [SerializeField] private int maxHealth;

        public float HealthPercentage => health / maxHealth;

        public int health { get; set; }

        private bool isDead => health == default;

        public void Prepare()
        {
            health = maxHealth;
        }

        public void TakeDamage(int damageAmount)
        {
            if (!isDead)
            {
                health = Mathf.Clamp(health - damageAmount, default, maxHealth);

                if (health < default(int))
                {
                    DeathEvent?.Invoke();
                }
            }
        }

        public void Heal(int healAmount)
        {
            if (!isDead)
            {
                health = Mathf.Clamp(health + healAmount, default, maxHealth);
            }
        }
    }

    [RequireComponent(typeof(PlayerInput))]
    public sealed class CarController : MonoBehaviour
    {
        [Header("Info")] [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private Vector3[] positionsOnRoad;

        [field: Header("Movement")]
        [field: SerializeField]
        public float speed { get; private set; } = 1.0f;

        [SerializeField] private float increaseSpeedRate = 0.1f;

        public float HealthPercentage => healthComponent.HealthPercentage;

        private int maxPositionIndex => positionsOnRoad.Length - 1;

        private Transform _transform;

        private int _currentPositionIndex;

        private void Awake()
        {
            _transform = transform;

            healthComponent.Prepare();
            healthComponent.DeathEvent += OnDeath;
        }

        private void Start()
        {
            _currentPositionIndex = Random.Range(default, positionsOnRoad.Length);
            _transform.position = positionsOnRoad[_currentPositionIndex];

            StartCoroutine(IncreaseSpeed());
        }

        private void Update()
        {
            var playerPosition = _transform.position;
            var newPosition = playerPosition;
            newPosition.x -= 5.0f;

            _transform.position = Vector3.Lerp(playerPosition, newPosition, speed * Time.deltaTime);
        }

        public void OnMoveDown(InputAction.CallbackContext ctx)
        {
            if (ctx.started && _currentPositionIndex != default)
            {
                _currentPositionIndex--;
                Move();
            }
        }

        public void OnMoveUp(InputAction.CallbackContext ctx)
        {
            if (ctx.started && _currentPositionIndex != maxPositionIndex)
            {
                _currentPositionIndex++;
                Move();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Move()
        {
            var playerPosition = _transform.position;
            var nextPosition = positionsOnRoad[_currentPositionIndex];
            nextPosition.x = playerPosition.x;
            _transform.position = nextPosition;
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