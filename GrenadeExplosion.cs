```csharp
using UnityEngine;
using System.Collections;

public class GrenadeExplosion : MonoBehaviour
{
    private float fuseTime;
    private float explosionRadius;
    private float explosionDamage;
    private GameObject explosionEffect;
    private AudioClip explosionSound;

    private bool hasExploded;

    public void Initialize(
        float fuse,
        float radius,
        float damage,
        GameObject effect,
        AudioClip sound)
    {
        fuseTime = fuse;
        explosionRadius = radius;
        explosionDamage = damage;
        explosionEffect = effect;
        explosionSound = sound;

        StartCoroutine(ExplosionCountdown());
    }

    IEnumerator ExplosionCountdown()
    {
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }

    void Explode()
    {
        if (hasExploded)
            return;

        hasExploded = true;

        Collider[] hitObjects = Physics.OverlapSphere(
            transform.position,
            explosionRadius
        );

        foreach (Collider hit in hitObjects)
        {
            TargetHealth target = hit.GetComponentInParent<TargetHealth>();

            if (target != null)
            {
                float distance = Vector3.Distance(
                    transform.position,
                    target.transform.position
                );

                float damageMultiplier = Mathf.Clamp01(
                    1f - distance / explosionRadius
                );

                target.TakeDamage(explosionDamage * damageMultiplier);
                continue;
            }

            EnemyAI enemy = hit.GetComponentInParent<EnemyAI>();

            if (enemy != null)
            {
                float distance = Vector3.Distance(
                    transform.position,
                    enemy.transform.position
                );

                float damageMultiplier = Mathf.Clamp01(
                    1f - distance / explosionRadius
                );

                enemy.TakeDamage(explosionDamage * damageMultiplier);
            }
        }

        if (explosionEffect != null)
        {
            Instantiate(
                explosionEffect,
                transform.position,
                Quaternion.identity
            );
        }

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(
                explosionSound,
                transform.position
            );
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
```