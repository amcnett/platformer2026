using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // components to get
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerInput pi;

    private Vector2 moveInput;
    private bool facingRight = true;
    private bool jumpPressed = false;
    private bool isGrounded = true;

    public float moveSpeed = 8f;
    public float jumpForce = 10f;
    [Tooltip("This is how much we want to reduce movement by.")]
    public float airMultiplier = .25f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public Transform shadowDot;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.25f;
    public float fallGravityScale = 2f; // update gravity to 200%

    [Header("Shooting")]
    public Transform firePoint;
    public GameObject fire; // our prefab to fire

    private float attackRate = 0.5f;
    private float nextAttackTime = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pi = GetComponent<PlayerInput>();
    }

    void OnMove(InputValue movementValue)
    {
        moveInput = movementValue.Get<Vector2>();
        Debug.Log("Player movement: " + moveInput.x);
    }

    void OnJump(InputValue movementValue)
    {
        jumpPressed = true;
        CheckGrounded(); // will update the isGrounded boolean
    }

/*    void OnAttack(InputValue attackValue)
    {
        anim.SetTrigger("isShooting"); // sets to true then false automatically
    }*/

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

        // event polling
        float isAttackHeld = pi.actions["Attack"].ReadValue<float>(); //1 means held, 0 means not held
        if (isAttackHeld > 0 && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate; //set this to future allow attack time
            anim.SetTrigger("isShooting");
            Instantiate(fire, firePoint.position, facingRight ? firePoint.rotation : Quaternion.Euler(0, 180, 0));
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput.x * moveSpeed; //how fast I want the player to go
        float speedDiff = targetSpeed - rb.linearVelocity.x; //how far away am I from the speed I want to be
        //float accelRate = moveSpeed;
        float accelRate = isGrounded ? moveSpeed : moveSpeed * airMultiplier;
        float movement = speedDiff * accelRate; //how hard to push the player

        rb.AddForce(Vector2.right * movement);
        anim.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));

        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpPressed = false;
            isGrounded = false;
        }

        // used to speed up player falling velocity
        if (rb.linearVelocity.y < 0) // player is falling
            rb.gravityScale = fallGravityScale; //set to 2 meaning 200%
        else
            rb.gravityScale = 1; //set back to 1 which is the default (100%)

        // check to see if we are hitting the ground from our raycast
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, 50f, groundLayer);
        if (hit)
        {
            shadowDot.position = hit.point;
            shadowDot.gameObject.SetActive(true);
        }

        // just for debugging
        Debug.DrawRay(rb.position, Vector2.down, Color.red, 1f);

    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1; // inverts the x value of the scale
        transform.localScale = theScale; //set game object scale equal to our modified scale
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        shadowDot.gameObject.SetActive(!isGrounded); //only show if not grounded
    }

    void OnDrawGizmos() // allows me to see how the circle is being drawn by viewing the scene view
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }

    public Vector2 GetDirection()
    {
        if (facingRight)
            return Vector2.right;
        else
            return Vector2.left;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("boundary"))
        {
            GameManager.instance.DecreaseLives();
            Debug.Log("Lives: " + GameManager.instance.GetLives());
            SceneManager.LoadScene(0);
        }
    }

}
