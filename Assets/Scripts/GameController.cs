using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;

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

    public GameObject gameOverScreen; // Reference to the Game Over screen
    public TMP_Text survivedText; // Reference to the Game Over text
    private int survivedLevelsCount;

    public static event Action OnGameOver; // Declare the event
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        progressAmount = 0;
        progressSlider.value = 0;
        Fruits.OnFruitCollected += IncreaseProgress;
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;
        PlayerHealth.OnPlayerDeath += GameOverScreen;  // Subscribe to the OnPlayerDeath event
        levelLoader.SetActive(false);  // Deactivate the level loader at the start
        gameOverScreen.SetActive(false);  // Deactivate the Game Over screen at the start 
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

    void GameOverScreen() { 
        gameOverScreen.SetActive(true);  // Show the Game Over screen
        survivedText.text = "You Reached Level " + (survivedLevelsCount+1) + "!";  // Update the text with the number of levels survived
    }

    public void RestartGame() {

        int currentLevel = currentLevelIndex;  // Store the current level index
        gameOverScreen.SetActive(false);
        survivedLevelsCount = 0;  // Reset the number of levels survived

        progressAmount = 0;
        progressSlider.value = 0;

        // Load the current level (not the first one)
        LoadLevel(currentLevel, false);

        LevelController levelController = levels[currentLevel].GetComponent<LevelController>();

        if (levelController != null)
        {
            levelController.RespawnCollectibles(); // Respawn the collectibles
        }

        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = true;

        Animator animator = player.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Rebind(); // reset all animator values
            animator.Update(0f); // forces update so it happens immediately
        }
    }
    void OnDestroy()
    {
        Fruits.OnFruitCollected -= IncreaseProgress;  // Unsubscribe when this object is destroyed
    }

    void LoadLevel(int level, bool wantSurvivedIncreased) {
        levelLoader.SetActive(false);

        levels[currentLevelIndex].gameObject.SetActive(false);
        levels[level].gameObject.SetActive(true);

        player.transform.position = new Vector3(-14, -8, 0);
        currentLevelIndex = level;

        progressAmount = 0;
        progressSlider.value = 0;
        if (wantSurvivedIncreased) {
            survivedLevelsCount++;  // Increment the number of levels survived
        }

        if (healthUI != null)
        {
            healthUI.SetMaxHearts(playerHealth.maxHealth);  // Reset hearts using the Player's max health
        }
        if (playerHealth != null)
        {
            playerHealth.ResetHealth();  // Call a method to reset the health
        }
    }
    void LoadNextLevel() { 
        int nextLevelIndex = (currentLevelIndex == levels.Count -1)? 0 : currentLevelIndex + 1;
        LoadLevel(nextLevelIndex, true);
    }
}
