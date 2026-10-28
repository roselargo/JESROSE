```csharp
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private int ammoAmount = 30;
    [SerializeField] private bool destroyAfterPickup = true;

    [Header("Effects")]
    [SerializeField] private AudioSource pickupSound;
    [SerializeField] private GameObject pickupEffect;

    private bool collected;

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        GunSystem gun =
            other.GetComponentInChildren<GunSystem>();

        if (gun == null)
        {
            gun = other.GetComponentInParent<GunSystem>();
        }

        if (gun == null)
            return;

        gun.AddAmmo(ammoAmount);
        collected = true;

        if (pickupEffect != null)
        {
            Instantiate(
                pickupEffect,
                transform.position,
                Quaternion.identity
            );
        }

        if (pickupSound != null)
        {
            pickupSound.transform.SetParent(null);
            pickupSound.Play();

            Destroy(
                pickupSound.gameObject,
                pickupSound.clip != null
                    ? pickupSound.clip.length
                    : 1f
            );
        }

        if (destroyAfterPickup)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }

        Debug.Log("Ammo collected: +" + ammoAmount);
    }
}
```