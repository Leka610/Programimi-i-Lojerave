using UnityEngine;
using TMPro;

public class SignTrigger : MonoBehaviour
{
    [TextArea]
    public string customMessage;

    public GameObject messagePanel;              // Drag in "SignMessagePanel"
    public TMP_Text messageText;          // Drag in "SignMessage"

    private void Start()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
        else
            Debug.LogWarning("Message panel not assigned!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && messagePanel != null)
        {
            messageText.text = customMessage;
            messagePanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }
}
