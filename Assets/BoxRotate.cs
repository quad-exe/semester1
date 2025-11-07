using System.Collections;
using UnityEngine;

public class BoxRotate2D : MonoBehaviour
{
    private bool inAir = false;
    private bool hasRotated = false;
    public float rotationDuration = 0.2f; // hvor lang tid rotationen skal tage

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            inAir = true;
            hasRotated = false; // tillad rotation én gang
            StartCoroutine(RotateSmoothlyAfterDelay(0.1f));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            inAir = false;
        }
    }

    IEnumerator RotateSmoothlyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (inAir && !hasRotated)
        {
            hasRotated = true;
            float elapsed = 0f;
            float startRotation = transform.eulerAngles.z;
            float targetRotation = startRotation - 90f;

            while (elapsed < rotationDuration)
            {
                elapsed += Time.deltaTime;
                float zRotation = Mathf.Lerp(startRotation, targetRotation, elapsed / rotationDuration);
                transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
                yield return null;
            }

            // Sørg for rotationen er præcis til slut
            transform.rotation = Quaternion.Euler(0f, 0f, targetRotation);
        }
    }
}
