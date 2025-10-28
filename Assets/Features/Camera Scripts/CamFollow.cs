using UnityEngine;

public class CameraFollowWithGhost : MonoBehaviour
{
    [Header("Targets")]
    public Transform playerOne;
    public Transform playerTwo;
    public GameObject ghostObject;  // Assign the ghost object

    [Header("Camera Settings")]
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    [Header("Zoom Settings")]
    public float minZoom = 5f;      // Zoom in when characters are close
    public float maxZoom = 10f;     // Zoom out when far apart
    public float zoomLimiter = 10f;

    [Header("Level Bounds")]
    public float minX, maxX, minY, maxY;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (playerOne == null || playerTwo == null)
            return;

        // --- Determine active targets ---
        Vector3 pos1 = playerOne.position;
        Vector3 pos2 = playerTwo.position;
        Vector3 posGhost = ghostObject != null && ghostObject.activeSelf ? ghostObject.transform.position : Vector3.zero;
        bool ghostActive = ghostObject != null && ghostObject.activeSelf;

        Vector3 centerPoint;

        if (ghostActive)
        {
            // Center between ghost and the other player (assumes two players)
            centerPoint = (pos1 + pos2 + posGhost) / 3f;
        }
        else
        {
            centerPoint = (pos1 + pos2) / 2f;
        }

        // --- Smooth camera movement ---
        Vector3 desiredPosition = centerPoint + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // --- Zoom based on distance ---
        float distance;
        if (ghostActive)
        {
            // Distance = max distance between any two of the three objects
            float d1 = Vector3.Distance(pos1, pos2);
            float d2 = Vector3.Distance(pos1, posGhost);
            float d3 = Vector3.Distance(pos2, posGhost);
            distance = Mathf.Max(d1, Mathf.Max(d2, d3));
        }
        else
        {
            distance = Vector3.Distance(pos1, pos2);
        }

        float targetZoom = Mathf.Lerp(minZoom, maxZoom, distance / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * 5f);

        // --- Clamp within bounds ---
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = cam.aspect * camHalfHeight;

        float clampedX = Mathf.Clamp(smoothedPosition.x, minX + camHalfWidth, maxX - camHalfWidth);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minY + camHalfHeight, maxY - camHalfHeight);

        transform.position = new Vector3(clampedX, clampedY, smoothedPosition.z);
    }
}



