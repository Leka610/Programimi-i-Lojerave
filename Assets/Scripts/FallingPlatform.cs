using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    public float fallWait = 1f;
    public float respawnDelay = 3f; // Time before platform resets
    public float resetFadeTime = 0.5f; // Optional fade in time
    public Vector3 respawnOffset = Vector3.zero; // Optional respawn offset if needed

    private bool isFalling;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Fall");
            StartCoroutine(FallCoroutine());
        }
    }

    private IEnumerator FallCoroutine()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallWait);
        rb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(respawnDelay);
        StartCoroutine(ResetPlatform());
    }

    private IEnumerator ResetPlatform()
    {
        // Optional: fade out/in or disable rendering during reset
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Static; // Disable physics

        transform.position = originalPosition + respawnOffset;
        transform.rotation = originalRotation;

        animator.Rebind(); // Reset animation state
        animator.Update(0f);

        yield return new WaitForSeconds(resetFadeTime);
        isFalling = false; // Ready to fall again
    }
}
