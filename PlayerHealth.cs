```csharp
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    public UnityEvent onPlayerDeath;
    public UnityEvent<float, float> onHealthChanged;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    public bool IsDead => CurrentHealth <= 0f;

    private void Awake()
    {
        maxHealth = Mathf.Max(1f, maxHealth);
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead || damage <= 0f)
            return;

        CurrentHealth = Mathf.Max(
            0f,
            CurrentHealth - damage
        );

        onHealthChanged?.Invoke(
            CurrentHealth,
            maxHealth
        );

        if (IsDead)
        {
            onPlayerDeath?.Invoke();
            Debug.Log("Player has been defeated!");
        }
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0f)
            return;

        CurrentHealth = Mathf.Min(
            maxHealth,
            CurrentHealth + amount
        );

        onHealthChanged?.Invoke(
            CurrentHealth,
            maxHealth
        );
    }

    public void ResetHealth()
    {
        CurrentHealth = maxHealth;

        onHealthChanged?.Invoke(
            CurrentHealth,
            maxHealth
        );
    }
}
```