using System;
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

    public sealed class CarController : MonoBehaviour
    {
        [Header("Info")] [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private Vector3[] positionsOnRoad;

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
        }

        public void OnMoveDown(InputAction.CallbackContext ctx)
        {
            if (ctx.started && _currentPositionIndex != default)
            {
                _currentPositionIndex--;
                _transform.position = positionsOnRoad[_currentPositionIndex];
            }
        }

        public void OnMoveUp(InputAction.CallbackContext ctx)
        {
            if (ctx.started && _currentPositionIndex != maxPositionIndex)
            {
                _currentPositionIndex++;
                _transform.position = positionsOnRoad[_currentPositionIndex];
            }
        }

        private void OnDeath()
        {
            healthComponent.DeathEvent -= OnDeath;
            
            // TODO: Call UI.
        }
    }
}