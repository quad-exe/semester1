using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerNumber = 1;
    private Rigidbody2D body;
    public float speed = 5f;
    public float jumpForce = 5f;
    public bool isJumping;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

        isJumping = false;
    }

    private void Update()
    {
        //body.linearVelocity = new Vector2(Input.GetAxis("Horizontal")*speed, body.linearVelocityY);

        //if(Input.GetKey(KeyCode.W))
        {
            //body.linearVelocity = new Vector2(body.linearVelocity.x, speed);

        }
        float move = 0f;

        if (playerNumber == 1)
        {
            if (Input.GetKey(KeyCode.A)) move = -1f;
            if (Input.GetKey(KeyCode.D)) move = 1f;

            if (!isJumping && Input.GetKeyDown(KeyCode.W)) Jump();

        }
        else if (playerNumber == 2)
        {
            if (Input.GetKey(KeyCode.LeftArrow)) move = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) move = 1f;

            //if (Input.GetKeyDown(KeyCode.UpArrow)) Jump();
        }

        body.linearVelocity = new Vector2(move * speed, body.linearVelocity.y);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isJumping = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isJumping = true;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        }
    }
        
}
