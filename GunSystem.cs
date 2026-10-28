```csharp
using UnityEngine;
using System.Collections;

public class GunSystem : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 8f;

    [Header("Ammo Settings")]
    public int magazineSize = 30;
    public int currentAmmo = 30;
    public int reserveAmmo = 120;
    public float reloadTime = 1.5f;

    [Header("Weapon Effects")]
    public Camera playerCamera;
    public ParticleSystem muzzleFlash;
    public AudioSource shootingSound;
    public AudioSource reloadSound;

    private float nextFireTime;
    private bool isReloading;

    void Start()
    {
        currentAmmo = Mathf.Clamp(currentAmmo, 0, magazineSize);

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        if (Input.GetButton("Fire1") &&
            Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (playerCamera == null)
        {
            Debug.LogWarning("GunSystem: Assign the player camera!");
            return;
        }

        nextFireTime = Time.time + 1f / fireRate;
        currentAmmo--;

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        if (shootingSound != null)
        {
            shootingSound.Play();
        }

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            TargetHealth target =
                hit.collider.GetComponentInParent<TargetHealth>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }
            else
            {
                EnemyAI enemy =
                    hit.collider.GetComponentInParent<EnemyAI>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }

            Debug.DrawLine(
                ray.origin,
                hit.point,
                Color.red,
                1f
            );
        }
    }

    IEnumerator Reload()
    {
        if (isReloading ||
            currentAmmo >= magazineSize ||
            reserveAmmo <= 0)
        {
            yield break;
        }

        isReloading = true;

        if (reloadSound != null)
        {
            reloadSound.Play();
        }

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = magazineSize - currentAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        isReloading = false;
    }

    public void AddAmmo(int amount)
    {
        if (amount > 0)
        {
            reserveAmmo += amount;
        }
    }
}
```