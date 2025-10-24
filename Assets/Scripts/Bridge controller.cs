using UnityEngine;

public class DrawbridgeController2D : MonoBehaviour
{
    [Header("Drawbridge Settings")]
    [Tooltip("The hinge or pivot point the bridge rotates around.")]
    public Transform hingePoint;

    [Tooltip("Angle when the bridge is raised (in degrees).")]
    public float raisedAngle = 0f;

    [Tooltip("Angle when the bridge is lowered (in degrees).")]
    public float loweredAngle = -70f;

    [Tooltip("How fast the bridge rotates (degrees per second).")]
    public float rotationSpeed = 40f;

    private bool isActive = false;
    private bool isMoving = false;
    private float currentAngle;

    void Start()
    {
        if (hingePoint == null)
        {
            Debug.LogError($"{name}: No hinge point assigned! Please assign a Transform as the pivot.");
            return;
        }

        // Start in raised position
        currentAngle = raisedAngle;
        hingePoint.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    void Update()
    {
        if (!isMoving || hingePoint == null) return;

        float targetAngle = isActive ? loweredAngle : raisedAngle;
        currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        // Apply rotation
        hingePoint.localRotation = Quaternion.Euler(0f, 0f, currentAngle);

        // Stop exactly at the target angle to prevent flicker
        if (Mathf.Abs(currentAngle - targetAngle) < 0.01f)
        {
            currentAngle = targetAngle;
            hingePoint.localRotation = Quaternion.Euler(0f, 0f, targetAngle);
            isMoving = false;
        }
    }

    public void SetActive(bool active)
    {
        if (isActive != active)
        {
            isActive = active;
            isMoving = true;
        }
    }
}



