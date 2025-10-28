using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalLocalPos; // The local position relative to its parent (important for follow cameras)
    private bool isShaking = false;

    public IEnumerator Shake(float duration, float magnitude)
    {
        if (isShaking) yield break; // prevent overlapping shakes
        isShaking = true;

        // Save current local position (important if camera moves)
        originalLocalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // random small offset
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalLocalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // restore to current follow position, not scene start
        transform.localPosition = originalLocalPos;
        isShaking = false;
    }
}
