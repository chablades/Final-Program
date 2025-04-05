using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        //Hozontal Speed
        rb.linearVelocity = new Vector2(horizontalInput * runSpeed, rb.linearVelocity.y);

        //flip sprite
        if (horizontalInput > 0.01f)
            transform.localScale = Vector2.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector2(-1,1);

        //Jump
        if(Input.GetKey(KeyCode.Space)){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, runSpeed);
        }
    }
}
