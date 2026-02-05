using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float bulletForce = 40f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 direction = player.GetComponent<PlayerController>().GetDirection();

        rb = GetComponent<Rigidbody2D>();
        //rb.AddForce(Vector2.right * bulletForce);

        //rb.linearVelocity = Vector2.right * bulletForce;
        rb.linearVelocity = direction * bulletForce;
        Invoke("Die", 3f);
    }

    void Die()
    {
        Destroy(gameObject); //don't use "this" as "this" is the component/script
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            Destroy(gameObject);
        }
    }


}
