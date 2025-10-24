using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PressurePlate : MonoBehaviour
{
    [Header("Bridge Connection")]
    public DrawbridgeController2D connectedBridge;

    [Header("Plate Settings")]
    public float pressDistance = 0.1f;
    public float pressSpeed = 4f;

    private Vector3 initialPosition;
    private Vector3 pressedPosition;
    private int objectsOnPlate = 0;

    void Start()
    {
        initialPosition = transform.position;
        pressedPosition = initialPosition + Vector3.down * pressDistance;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger)
            col.isTrigger = true;
    }

    void Update()
    {
        Vector3 target = objectsOnPlate > 0 ? pressedPosition : initialPosition;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * pressSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsValidObject(collision))
        {
            objectsOnPlate++;
            connectedBridge?.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsValidObject(collision))
        {
            objectsOnPlate = Mathf.Max(0, objectsOnPlate - 1);
            if (objectsOnPlate == 0)
                connectedBridge?.SetActive(false);
        }
    }

    private bool IsValidObject(Collider2D col)
    {
        return col.CompareTag("Player2") || col.CompareTag("Box");
    }
}

