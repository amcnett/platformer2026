using System.Collections;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public float distance = 3f;
    public float movement =.25f;
    private int counter = 5; // how many times to move in a give direction
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("MoveObject");
    }

    IEnumerator MoveObject()
    {
        while (true)
        {
            transform.Translate(new Vector2(0, distance * movement));
            counter--;

            if (counter <= 0) // if we moved 5 times
            {
                movement *= -1; // now move in opposite direction
                counter = 5; // reset our counter
            }

            yield return new WaitForSeconds(.25f);
        }
    }
}
