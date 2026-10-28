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
        // Kung walang nakalagay sa Inspector, awtomatikong hahanapin ang GunSystem
        if (gunSystem == null)
        {
            gunSystem = FindObjectOfType<GunSystem>();
        }

        if (gunSystem == null)
        {
            Debug.LogError("GunAmmo: GunSystem reference is missing! Please attach GunSystem.");
            enabled = false;
            return;
        }

        // Unang pag-update ng text pagka-start ng laro
        UpdateAmmoDisplay();
    }

    private void Update()
    {
        // Palaging binabantayan ang bilang ng bala bawat frame
        UpdateAmmoDisplay();
    }

    private void UpdateAmmoDisplay()
    {
        // Siguraduhing may nakakabit na GunSystem at Text bago mag-update para walang error
        if (gunSystem == null || ammoText == null)
            return;

        // Kinukuha ang kasalukuyang data ng bala mula sa iyong GunSystem script
        int magazineAmmo = gunSystem.currentAmmo;
        int reserveAmmo = gunSystem.reserveAmmo;

        // Kung hindi naman nagbago ang bilang ng bala, hindi na i-update ang text (Para iwas lag)
        if (magazineAmmo == previousMagazineAmmo && reserveAmmo == previousReserveAmmo)
        {
            return;
        }

        // I-save ang kasalukuyang bilang bilang lumang bilang
        previousMagazineAmmo = magazineAmmo;
        previousReserveAmmo = reserveAmmo;

        // Pinagsasama ang Magazine Ammo, Separator, at Reserve Ammo sa screen (Halimbawa: 30 / 90)
        ammoText.text = magazineAmmo.ToString() + separator + reserveAmmo.ToString();

        // Warning system: Magiging kulay pula ang text kung 5 o mas mababa na lang ang bala mo
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
