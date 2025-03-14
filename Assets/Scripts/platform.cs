using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private Material stableMaterial; // Material for stable platforms
    [SerializeField] private Material unstableMaterial; // Material for unstable platforms
    [SerializeField] private ParticleSystem breakParticles; // Particles for breaking

    [Header("Behavior Settings")]
    [SerializeField] private bool isUnstable = false; // Is the platform unstable?
    [SerializeField] private float breakDelay = 0.5f; // Delay before breaking
    [SerializeField] private float moveSpeed = 2f; // Speed of moving platforms
    [SerializeField] private Vector3 moveDirection = Vector3.right; // Direction of movement

    private Renderer platformRenderer;
    private Collider platformCollider;
    private Vector3 startPosition;
    private bool isBreaking;

    private void Awake()
    {
        platformRenderer = GetComponent<Renderer>();
        platformCollider = GetComponent<Collider>();
        startPosition = transform.position;
    }

    private void Update()
    {
        // Handle moving platforms
        if (isUnstable)
        {
            MovePlatform();
        }
    }

    /// <summary>
    /// Sets the platform as stable or unstable.
    /// </summary>
    public void SetUnstable(bool isUnstable)
    {
        this.isUnstable = isUnstable;
        platformRenderer.material = isUnstable ? unstableMaterial : stableMaterial;
    }

    /// <summary>
    /// Handles platform movement.
    /// </summary>
    private void MovePlatform()
    {
        float pingPong = Mathf.PingPong(Time.time * moveSpeed, 1);
        transform.position = startPosition + moveDirection * pingPong;
    }

    /// <summary>
    /// Breaks the platform if it's unstable.
    /// </summary>
    public void BreakPlatform()
    {
        if (isUnstable && !isBreaking)
        {
            isBreaking = true;
            Invoke(nameof(DisablePlatform), breakDelay);

            // Play break particles
            if (breakParticles != null)
            {
                breakParticles.Play();
            }
        }
    }

    /// <summary>
    /// Disables the platform.
    /// </summary>
    private void DisablePlatform()
    {
        gameObject.SetActive(false);
        isBreaking = false;
    }
}