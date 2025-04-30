using UnityEngine;
using System;

public class Fruits : MonoBehaviour, IItems
{
    public static event Action<int> OnFruitCollected;
    public int worth = 5;
    private bool isCollected = false;  // Flag to track if fruit is already collected

    public void Collect()
    {
        if (!isCollected)
        {
            isCollected = true;  // Mark as collected
            OnFruitCollected?.Invoke(worth);  // Trigger event
            Destroy(gameObject);  // Destroy the fruit
        }
    }
}