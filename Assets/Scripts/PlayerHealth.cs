using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    private bool isInvincible = false;
    public float invincibilityDuration = 1f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Animator animator;
    public HealthUI healthUI;

    void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
        Debug.Log("Calling SetMaxHearts with: " + maxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount, Vector2 hitDirection, float knockbackForce = 8f)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        healthUI.UpdateHearts(currentHealth);
        Debug.Log("Player hit");

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        ApplyKnockback(hitDirection, knockbackForce);
        StartCoroutine(FlashRedAndInvincible());
    }

    private void ApplyKnockback(Vector2 direction, float force)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Stop any current motion
            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }
    }

    void Die()
    {
        animator.SetTrigger("Dead");
        Debug.Log("Player died!");

        // Freeze horizontal movement by setting the x-velocity to 0, but let gravity affect vertical movement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);  // Set x velocity to 0 but keep the y velocity for gravity
        }

        // Optionally, disable the movement script if you have one
        PlayerMovement movementScript = GetComponent<PlayerMovement>();
        if (movementScript != null)
        {
            movementScript.enabled = false;  // Disable the movement script to stop inputs
        }

        // Additional logic for death (e.g., game over, respawn, etc.)
    }

    private System.Collections.IEnumerator FlashRedAndInvincible()
    {
        isInvincible = true;
        spriteRenderer.color = new Color(1f, 0f, 0f, 0.75f);

        yield return new WaitForSeconds(invincibilityDuration);

        spriteRenderer.color = originalColor;
        isInvincible = false;
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;  // Reset the player's health to max
        healthUI.UpdateHearts(currentHealth);  // Update the UI to reflect the new health
    }
}
