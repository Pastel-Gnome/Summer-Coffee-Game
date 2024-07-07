using UnityEngine;
using System.Collections;

public class ScaleTween : MonoBehaviour
{
    public float targetScaleY = 0.3191134f;
    public float duration = 2f;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private float originalHeight;

    public void StartPouring()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
        originalHeight = transform.localScale.y;

        StartCoroutine(ScaleYOverTime(targetScaleY, duration));
    }

    IEnumerator ScaleYOverTime(float targetScaleY, float duration)
    {
        float currentTime = 0f;

        while (currentTime <= duration)
        {
            float t = currentTime / duration;
            float currentScaleY = Mathf.Lerp(0, targetScaleY, t);

            // Scale the object from the top
            Vector3 newScale = originalScale;
            newScale.y = currentScaleY;
            transform.localScale = newScale;

            // Adjust position to keep the top at the same level
            Vector3 newPosition = originalPosition + Vector3.up * (originalHeight - currentScaleY);
            transform.position = newPosition;

            currentTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final scale and position
        transform.localScale = new Vector3(originalScale.x, targetScaleY, originalScale.z);
        transform.position = originalPosition - Vector3.up * (originalHeight - targetScaleY);
    }
}
