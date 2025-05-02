using UnityEngine;

public class FanTrap : MonoBehaviour
{
    public float bounceForce = 10f; // The force applied to the player when they hit the fan trap

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) { 
            HandlePlayerBounce(collision.gameObject);
        }
    }

    private void HandlePlayerBounce(GameObject player) { 
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if(rb != null) { 
            Vector2 bounceDirection = new Vector2(rb.linearVelocity.x, 0f); // Bounce upwards
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}
