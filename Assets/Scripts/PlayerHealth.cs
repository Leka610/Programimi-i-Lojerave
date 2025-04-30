using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private bool isInvincible = false;
    public float invincibilityDuration = 1f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount, Vector2 hitDirection, float knockbackForce = 8f)
    {
        if (isInvincible) return;

        currentHealth -= amount;
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
        // Add death logic (reload scene, show game over, etc.)
    }

    private System.Collections.IEnumerator FlashRedAndInvincible()
    {
        isInvincible = true;
        spriteRenderer.color = new Color(1f, 0f, 0f, 0.75f);

        yield return new WaitForSeconds(invincibilityDuration);

        spriteRenderer.color = originalColor;
        isInvincible = false;
    }
}
