```csharp
using UnityEngine;
using System.Collections;

public class GrenadeSystem : MonoBehaviour
{
    [Header("Grenade Settings")]
    public GameObject grenadePrefab;
    public Transform throwPoint;
    public float throwForce = 15f;
    public float upwardForce = 3f;
    public float fuseTime = 3f;

    [Header("Explosion Settings")]
    public float explosionRadius = 5f;
    public float explosionDamage = 100f;
    public GameObject explosionEffect;
    public AudioClip explosionSound;

    [Header("Ammo")]
    public int grenadeCount = 3;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        // Press G to throw a grenade
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
        }
    }

    public void ThrowGrenade()
    {
        if (grenadeCount <= 0)
        {
            Debug.Log("Walay grenade!");
            return;
        }

        if (grenadePrefab == null || throwPoint == null)
        {
            Debug.LogWarning("I-assign ang Grenade Prefab ug Throw Point!");
            return;
        }

        GameObject grenade = Instantiate(
            grenadePrefab,
            throwPoint.position,
            throwPoint.rotation
        );

        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = playerCamera != null
                ? playerCamera.transform.forward
                : throwPoint.forward;

            rb.AddForce(
                direction * throwForce + Vector3.up * upwardForce,
                ForceMode.Impulse
            );
        }

        GrenadeExplosion explosion =
            grenade.GetComponent<GrenadeExplosion>();

        if (explosion != null)
        {
            explosion.Initialize(
                fuseTime,
                explosionRadius,
                explosionDamage,
                explosionEffect,
                explosionSound
            );
        }

        grenadeCount--;
    }
}
```