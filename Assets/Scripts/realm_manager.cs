using UnityEngine;
using System.Collections;

public class RealmManager : MonoBehaviour
{
    public static RealmManager Instance;

    [Header("Realm Settings")]
    [SerializeField] private float realmTransitionDuration = 1f; // Duration of realm transition
    [SerializeField] private float screenShakeDuration = 0.5f; // Duration of screen shake
    [SerializeField] private float screenShakeMagnitude = 0.1f; // Intensity of screen shake

    private bool isLightWorld = true;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Toggles between light and shadow realms.
    /// </summary>
    public void ToggleRealm()
    {
        isLightWorld = !isLightWorld;
        StartCoroutine(TransitionRealm());
        StartCoroutine(ScreenShake(screenShakeDuration, screenShakeMagnitude));
    }

    /// <summary>
    /// Smoothly transitions the realm.
    /// </summary>
    private IEnumerator TransitionRealm()
    {
        // Add frustrating effects (e.g., delayed transition)
        yield return new WaitForSeconds(0.5f);

        // Update realm objects
        UpdateWorldState();
    }

    /// <summary>
    /// Updates the state of all realm objects.
    /// </summary>
    private void UpdateWorldState()
    {
        // Update platforms and other realm objects
    }

    /// <summary>
    /// Shakes the camera for a specified duration and magnitude.
    /// </summary>
    public IEnumerator ScreenShake(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }
}