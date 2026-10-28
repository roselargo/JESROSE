```csharp
using UnityEngine;
using TMPro;

public class GunAmmo : MonoBehaviour
{
    [Header("Weapon Reference")]
    [SerializeField] private GunSystem gunSystem;

    [Header("UI References")]
    [SerializeField] private TMP_Text ammoText;

    [Header("Display Settings")]
    [SerializeField] private string separator = " / ";

    private int previousMagazineAmmo = -1;
    private int previousReserveAmmo = -1;

    private void Start()
    {
        if (gunSystem == null)
        {
            gunSystem = FindObjectOfType<GunSystem>();
        }

        if (gunSystem == null)
        {
            Debug.LogError(
                "GunAmmo: GunSystem reference is missing!"
            );

            enabled = false;
            return;
        }

        UpdateAmmoDisplay();
    }

    private void Update()
    {
        UpdateAmmoDisplay();
    }

    private void UpdateAmmoDisplay()
    {
        if (gunSystem == null || ammoText == null)
            return;

        int magazineAmmo = gunSystem.currentAmmo;
        int reserveAmmo = gunSystem.reserveAmmo;

        if (magazineAmmo == previousMagazineAmmo &&
            reserveAmmo == previousReserveAmmo)
        {
            return;
        }

        previousMagazineAmmo = magazineAmmo;
        previousReserveAmmo = reserveAmmo;

        ammoText.text =
            magazineAmmo.ToString() +
            separator +
            reserveAmmo.ToString();

        if (magazineAmmo <= 5)
        {
            ammoText.color = Color.red;
        }
        else
        {
            ammoText.color = Color.white;
        }
    }
}
```