using UnityEngine;
using System.Collections;

public class WeaponReloadAnimation : MonoBehaviour
{
    [Header("Reload Animation Settings")]
    public Vector3 reloadPosition = new Vector3(0f, -0.25f, -0.15f);
    public Vector3 reloadRotation = new Vector3(25f, 0f, 10f);

    [Header("Animation Duration (Seconds)")]
    public float reloadDuration = 1.5f; // Dito mo pwedeng baguhin kung gaano katagal ang reload sa Unity Inspector

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isReloading;

    void Start()
    {
        // Awtomatikong sine-save ang orihinal na pwesto ng baril pagka-start ng laro
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        // Kusa itong gagana kapag pinindot ang 'R' key at hindi pa kasalukuyang nagre-reload
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            PlayReloadAnimation(reloadDuration);
        }
    }

    public void PlayReloadAnimation(float duration = 1.5f)
    {
        if (isReloading)
            return;

        StartCoroutine(ReloadAnimation(duration));
    }

    IEnumerator ReloadAnimation(float duration)
    {
        isReloading = true;

        float halfDuration = duration / 2f;

        // 1. Ibebenta o ibababa ang armas gamit ang reload position at rotation
        yield return MoveWeapon(
            originalPosition + reloadPosition,
            Quaternion.Euler(reloadRotation) * originalRotation,
            halfDuration
        );

        // 2. Ibabalik ang armas sa orihinal nitong posisyon
        yield return MoveWeapon(
            originalPosition,
            originalRotation,
            halfDuration
        );

        // Siguraduhing saktong bumalik sa simula
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;

        isReloading = false;
    }

    IEnumerator MoveWeapon(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.localRotation;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(timer / duration);
            progress = Mathf.SmoothStep(0f, 1f, progress); // Swabe na transition gamit ang SmoothStep

            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, progress);
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, progress);

            yield return null; // Hihintayin ang susunod na frame bago mag-loop ulit
        }

        transform.localPosition = targetPosition;
        transform.localRotation = targetRotation;
    }
}
