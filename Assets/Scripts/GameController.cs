using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    int progressAmount;
    public Slider progressSlider;

    public GameObject player;
    public GameObject levelLoader;
    public List <GameObject> levels;
    private int currentLevelIndex = 0;
    public HealthUI healthUI;
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        progressAmount = 0;
        progressSlider.value = 0;
        Fruits.OnFruitCollected += IncreaseProgress;
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;
        levelLoader.SetActive(false);  // Deactivate the level loader at the start
    }
    void IncreaseProgress(int amount) {
        progressAmount += amount;
        progressSlider.value = progressAmount;
        Debug.Log("Progress: " + progressAmount);
        if (progressAmount >= 100)
        {
            levelLoader.SetActive(true);  // Activate the level loader
            Debug.Log("You win!");
        }
    }
    void OnDestroy()
    {
        Fruits.OnFruitCollected -= IncreaseProgress;  // Unsubscribe when this object is destroyed
    }
    void LoadNextLevel() { 
        int nextLevelIndex = (currentLevelIndex == levels.Count -1)? 0 : currentLevelIndex + 1;
        levelLoader.SetActive(false);

        levels[currentLevelIndex].gameObject.SetActive(false);
        levels[nextLevelIndex].gameObject.SetActive(true);

        player.transform.position = new Vector3(-14,-8,0);
        currentLevelIndex = nextLevelIndex;

        progressAmount = 0;
        progressSlider.value = 0;

        if (healthUI != null)
        {
            healthUI.SetMaxHearts(playerHealth.maxHealth);  // Reset hearts using the Player's max health
        }
        if (playerHealth != null)
        {
            playerHealth.ResetHealth();  // Call a method to reset the health
        }
    }
}
