using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class DragonController : MonoBehaviour
{
    //private Vector2 pointA;
    //private Vector2 pointB;
    //public float speed = 2f;
    //public float duration = 3f;
    private float health = 100;

    public float distance = 3f;
    public float distanceCheck = 2f;
    public float movement = 1f;
    public LayerMask ground;
    //private int counter = 5; // how many times to move in a give direction

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //pointA = transform.position;
        //pointB = new Vector2(transform.position.x + 5, transform.position.y);
        StartCoroutine("MoveObject");
    }

    // Update is called once per frame
    void Update()
    {
        //float t = Mathf.PingPong(Time.time * speed, 1f);
        //float t = Mathf.PingPong(Time.time/duration, 1f);
        //transform.position = Vector2.Lerp(pointA, pointB, t);
    }

    IEnumerator MoveObject()
    {
        while (true)
        {
            transform.Translate(new Vector2(distance * movement, 0));
            //counter--;

            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, distanceCheck, ground); // we hit floor below
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, distanceCheck, ground); // we hit a wall to the left
            Debug.DrawRay(transform.position, Vector2.left * distanceCheck, Color.red, 1f); //draws line in scene for 1 second
            Debug.DrawRay(transform.position, Vector2.right * distanceCheck, Color.red, 1f); //draws line in scene for 1 second

            if (!hitDown || hitLeft) //  for when we used lerp method -> if we moved 5 times
            {
                movement *= -1; // now move in opposite direction
                //counter = 5; // reset our counter (used for lerp method)
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("fire"))
            health += -25;

        if (health <= 0)
            Destroy(gameObject);
    }

}
