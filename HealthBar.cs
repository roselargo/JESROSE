```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("UI Components")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text healthText;

    [Header("Visual Settings")]
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private bool showHealthText = true;

    private void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }

        if (healthSlider == null)
        {
            healthSlider = GetComponent<Slider>();
        }

        if (playerHealth == null)
        {
            Debug.LogError(
                "HealthBar: PlayerHealth reference is missing!"
            );
            return;
        }

        playerHealth.onHealthChanged.AddListener(UpdateHealthBar);

        UpdateHealthBar(
            playerHealth.CurrentHealth,
            playerHealth.MaxHealth
        );
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float percentage = maxHealth > 0f
            ? Mathf.Clamp01(currentHealth / maxHealth)
            : 0f;

        if (healthSlider != null)
        {
            healthSlider.maxValue = 1f;
            healthSlider.value = percentage;
        }

        if (fillImage != null && healthGradient != null)
        {
            fillImage.color = healthGradient.Evaluate(percentage);
        }

        if (healthText != null && showHealthText)
        {
            healthText.text =
                Mathf.CeilToInt(currentHealth) + " / " +
                Mathf.CeilToInt(maxHealth);
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.onHealthChanged.RemoveListener(
                UpdateHealthBar
            );
        }
    }
}
```