using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{
    public Image heartPrefab;
    public Sprite fullHeartSprite;

    private List<Image> hearts = new List<Image>();
    private Color fullHeartColor = new Color(0.765f, 0.169f, 0.263f);

    public void SetMaxHearts(int maxHearts)
    {
        foreach (Image heart in hearts)
        {
            Destroy(heart.gameObject);
        }

        hearts.Clear();

        for (int i = 0; i < maxHearts; i++)
        {
            Image newHeart = Instantiate(heartPrefab, transform);
            newHeart.sprite = fullHeartSprite;
            newHeart.color = fullHeartColor;
            hearts.Add(newHeart);
            Debug.Log("Heart added");
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            bool shouldBeVisible = i < currentHealth;
            hearts[i].gameObject.SetActive(shouldBeVisible);
        }
    }
}
