```csharp
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    public float recoilX = 2f;
    public float recoilY = 1f;
    public float recoilZ = 0.2f;

    [Header("Recovery")]
    public float returnSpeed = 5f;
    public float snappiness = 10f;

    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        targetRotation = Vector3.Lerp(
            targetRotation,
            Vector3.zero,
            returnSpeed * Time.deltaTime
        );

        currentRotation = Vector3.Slerp(
            currentRotation,
            targetRotation,
            snappiness * Time.deltaTime
        );

        transform.localRotation = Quaternion.Euler(currentRotation);
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            initialPosition,
            returnSpeed * Time.deltaTime
        );
    }

    public void ApplyRecoil()
    {
        targetRotation += new Vector3(
            -recoilX,
            Random.Range(-recoilY, recoilY),
            Random.Range(-recoilZ, recoilZ)
        );
    }
}
```