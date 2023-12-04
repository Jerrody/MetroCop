using System;
using UnityEngine;

namespace Cars
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


    public abstract class CarController : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] protected HealthComponent healthComponent;
    }
}