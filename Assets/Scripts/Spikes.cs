using UnityEngine;

public class Spikes : MonoBehaviour
{
    public int damageAmount = 1; // How much damage the player takes
    public float knockbackForce = 5f; // How much knockback to apply to the player

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collides with the spikes
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                Vector2 knockbackDirection = other.transform.position - transform.position;
                player.TakeDamage(damageAmount, Vector2.up); // test vertical knockback
                Debug.Log("Knockback Dir: " + knockbackDirection);
            }
        }
    }

    // Optional: Use OnTriggerStay2D if you want to keep dealing damage when the player is still inside the spikes' area
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                Vector2 knockbackDirection = other.transform.position - transform.position;
                player.TakeDamage(damageAmount, Vector2.up); // test vertical knockback
                Debug.Log("Knockback Dir: " + knockbackDirection);
            }
        }
    }
}
