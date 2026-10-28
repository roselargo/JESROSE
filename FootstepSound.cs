```csharp
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootstepSound : MonoBehaviour
{
    [Header("Footstep Audio")]
    public AudioSource audioSource;
    public AudioClip[] footstepSounds;

    [Header("Movement Settings")]
    public float walkInterval = 0.5f;
    public float sprintInterval = 0.3f;
    public float minimumMoveSpeed = 0.2f;

    private CharacterController controller;
    private float stepTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!controller.isGrounded)
            return;

        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        if (horizontalVelocity.magnitude < minimumMoveSpeed)
        {
            stepTimer = 0f;
            return;
        }

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        float interval = isSprinting ? sprintInterval : walkInterval;

        stepTimer += Time.deltaTime;

        if (stepTimer >= interval)
        {
            PlayFootstep();
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        if (audioSource == null || footstepSounds == null ||
            footstepSounds.Length == 0)
            return;

        AudioClip clip = footstepSounds[
            Random.Range(0, footstepSounds.Length)
        ];

        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
```