using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool facingRight = true;
    private bool jumpPressed = false;

    public float moveSpeed = 8f;
    public float jumpForce = 10f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue movementValue)
    {
        moveInput = movementValue.Get<Vector2>();
        Debug.Log("Player movement: " + moveInput.x);
    }

    void OnJump(InputValue movementValue)
    {
        jumpPressed = true;
    }

    // Update is called once per frame
    void Update()
    {
        //one way to move game objects = by using translate
        //transform.Translate(moveInput * Vector2.left * Time.deltaTime);
        if (moveInput.x < 0 && facingRight) //moving left but facing right
        {
            Flip();
            facingRight = false;
        }
        else if (moveInput.x > 0 && !facingRight)  //moving right but facing left
        {
            Flip();
            facingRight = true;
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput.x * moveSpeed; //how fast I want the player to go
        float speedDiff = targetSpeed - rb.linearVelocity.x; //how far away am I from the speed I want to be
        float accelRate = moveSpeed;
        float movement = speedDiff * accelRate; //how hard to push the player

        rb.AddForce(Vector2.right * movement);
        anim.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));

        if (jumpPressed)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpPressed = false;
        }
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1; // inverts the x value of the scale
        transform.localScale = theScale; //set game object scale equal to our modified scale
    }

}
