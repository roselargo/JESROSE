```csharp
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageIndicator : MonoBehaviour
{
    [Header("UI Settings")]
    public Image damageOverlay;
    public float flashDuration = 0.25f;
    public float maxAlpha = 0.65f;

    private PlayerHealth playerHealth;
    private Coroutine flashCoroutine;
    private float previousHealth;

    void Start()
    {
        if (damageOverlay != null)
        {
            Color color = damageOverlay.color;
            color.a = 0f;
            damageOverlay.color = color;
        }

        playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth != null)
        {
            previousHealth = playerHealth.CurrentHealth;
            playerHealth.onHealthChanged.AddListener(OnHealthChanged);
        }
        else
        {
            Debug.LogWarning("DamageIndicator: PlayerHealth not found!");
        }
    }

    void OnHealthChanged(float currentHealth, float maxHealth)
    {
        if (currentHealth < previousHealth)
        {
            ShowDamageFlash();
        }

        previousHealth = currentHealth;
    }

    public void ShowDamageFlash()
    {
        if (damageOverlay == null)
            return;

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        Color color = damageOverlay.color;
        color.a = maxAlpha;
        damageOverlay.color = color;

        float timer = 0f;

        while (timer < flashDuration)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(maxAlpha, 0f, timer / flashDuration);
            damageOverlay.color = color;

            yield return null;
        }

        color.a = 0f;
        damageOverlay.color = color;
        flashCoroutine = null;
    }

    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.onHealthChanged.RemoveListener(OnHealthChanged);
        }
    }
}
```