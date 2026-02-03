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

    public float distance = 3f;
    public float movement = .25f;
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

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
            Debug.DrawRay(transform.position, Vector2.down, Color.red, 1f); //draws line in scene for 1 second

            if (!hit) // if we moved 5 times
            {
                movement *= -1; // now move in opposite direction
                //counter = 5; // reset our counter
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
}
