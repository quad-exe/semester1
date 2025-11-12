using UnityEngine;

public class PlatformFalling : MonoBehaviour
{
    public GameObject platform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Square"))
        {
            Destroy(platform);
        }
    }
}
