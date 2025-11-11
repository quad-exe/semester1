using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxTriggerPoint : MonoBehaviour
{
    public Rigidbody2D boxRigidbody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Circle") || collision.CompareTag("Square"))
        {
           
            Dead();
            
        }
    }
    void Dead()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
