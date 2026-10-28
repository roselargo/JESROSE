```csharp
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [Header("Safe Zone Settings")]
    [SerializeField] private bool enableHealing = true;
    [SerializeField] private float healingPerSecond = 5f;

    [Header("Zone Effects")]
    [SerializeField] private GameObject zoneEffect;

    private void OnTriggerStay(Collider other)
    {
        if (!enableHealing)
            return;

        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player != null && !player.IsDead)
        {
            player.Heal(
                healingPerSecond * Time.deltaTime
            );
        }
    }

    private void OnDrawGizmos()
    {
        Collider zoneCollider = GetComponent<Collider>();

        if (zoneCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                zoneCollider.bounds.center,
                zoneCollider.bounds.size
            );
        }
    }
}
```