```csharp
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private int startingWeapon = 0;

    private int currentWeaponIndex;

    private void Start()
    {
        if (weapons == null || weapons.Length == 0)
        {
            Debug.LogWarning("No weapons assigned!");
            return;
        }

        currentWeaponIndex = Mathf.Clamp(
            startingWeapon,
            0,
            weapons.Length - 1
        );

        SelectWeapon(currentWeaponIndex);
    }

    private void Update()
    {
        if (weapons == null || weapons.Length == 0)
            return;

        for (int i = 0; i < Mathf.Min(weapons.Length, 9); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectWeapon(i);
            }
        }

        float scroll = Input.mouseScrollDelta.y;

        if (scroll > 0f)
        {
            SelectWeapon(
                (currentWeaponIndex + 1) % weapons.Length
            );
        }
        else if (scroll < 0f)
        {
            SelectWeapon(
                (currentWeaponIndex - 1 + weapons.Length)
                % weapons.Length
            );
        }
    }

    public void SelectWeapon(int index)
    {
        if (weapons == null ||
            index < 0 ||
            index >= weapons.Length)
        {
            return;
        }

        currentWeaponIndex = index;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                weapons[i].SetActive(i == index);
            }
        }
    }
}
```