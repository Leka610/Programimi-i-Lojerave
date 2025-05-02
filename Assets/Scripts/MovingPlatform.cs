using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA; // The starting point of the platform
    public Transform pointB; // The ending point of the platform
    public float speed = 2.0f; // Speed of the platform movement
    private Vector3 nextPosition; // Reference to the Effector2D component
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextPosition = pointB.position; // Initialize the next position to point A   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime); // Move the platform towards the next position

        if (transform.position == nextPosition)
        {
            nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position; // Switch the next position between point A and point B
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform); // Set the player as a child of the platform
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // Remove the player from the platform when they exit
        }
    }
}
