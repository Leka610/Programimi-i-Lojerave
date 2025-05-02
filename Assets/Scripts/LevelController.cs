using UnityEngine;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
    public GameObject itemsParent; // The parent GameObject containing the fruits (collectibles)
    public GameObject bananaPrefab; // Reference to the Banana prefab
    public GameObject cherryPrefab; // Reference to the Cherry prefab
    public GameObject strawberryPrefab; // Reference to the Strawberry prefab
    public GameObject applePrefab; // Reference to the Apple prefab
    public GameObject orangePrefab;

    private List<Vector3> bananaPositions = new List<Vector3>(); // List to hold positions for respawning bananas
    private List<Vector3> cherryPositions = new List<Vector3>(); // List to hold positions for respawning cherries
    private List<Vector3> strawberryPositions = new List<Vector3>(); // List to hold positions for respawning strawberries
    private List<Vector3> applePositions = new List<Vector3>(); // List to hold positions for respawning apples
    private List<Vector3> orangePositions = new List<Vector3>(); // List to hold positions for respawning oranges

    void Start()
    {
        // Ensure that fruits are created at the start of the game
        InitializeCollectibles();
    }

    private void InitializeCollectibles()
    {
        // Find the Bananas and Cherries GameObjects under Items
        Transform bananasParent = itemsParent.transform.Find("Bananas");
        Transform cherriesParent = itemsParent.transform.Find("Cherries");
        Transform strawberriesParent = itemsParent.transform.Find("Strawberries");
        Transform applesParent = itemsParent.transform.Find("Apples");
        Transform orangesParent = itemsParent.transform.Find("Oranges");

        // Store initial positions of bananas and cherries and destroy them (since we'll respawn them)
        if (bananasParent != null)
        {
            foreach (Transform fruit in bananasParent)
            {
                bananaPositions.Add(fruit.position); // Store position
                Destroy(fruit.gameObject); // Destroy the initial instance
            }
        }

        if (cherriesParent != null)
        {
            foreach (Transform fruit in cherriesParent)
            {
                cherryPositions.Add(fruit.position); // Store position
                Destroy(fruit.gameObject); // Destroy the initial instance
            }
        }

        if (strawberriesParent != null)
        {
            foreach (Transform fruit in strawberriesParent)
            {
                strawberryPositions.Add(fruit.position); // Store position
                Destroy(fruit.gameObject); // Destroy the initial instance
            }
        }

        if (applesParent != null)
        {
            foreach (Transform fruit in applesParent)
            {
                applePositions.Add(fruit.position); // Store position
                // Assuming you want to destroy apples as well, add similar logic here
                Destroy(fruit.gameObject); // Destroy the initial instance
            }
        }

        if (orangesParent != null)
        {
            foreach (Transform fruit in orangesParent)
            {
                orangePositions.Add(fruit.position); // Store position
                Destroy(fruit.gameObject); // Destroy the initial instance
            }
        }

        // Now, instantiate the fruits at their original positions when the game starts
        RespawnCollectibles();
    }

    public void RespawnCollectibles()
    {
        // Respawn the bananas at their original positions
        for (int i = 0; i < bananaPositions.Count; i++)
        {
            Instantiate(bananaPrefab, bananaPositions[i], Quaternion.identity, itemsParent.transform);
        }

        // Respawn the cherries at their original positions
        for (int i = 0; i < cherryPositions.Count; i++)
        {
            Instantiate(cherryPrefab, cherryPositions[i], Quaternion.identity, itemsParent.transform);
        }

        // Respawn the strawberries at their original positions
        for (int i = 0; i < strawberryPositions.Count; i++)
        {
            Instantiate(strawberryPrefab, strawberryPositions[i], Quaternion.identity, itemsParent.transform);
        }

        // Respawn the apples at their original positions (if needed)
        for (int i = 0; i < applePositions.Count; i++)
        {
            Instantiate(applePrefab, applePositions[i], Quaternion.identity, itemsParent.transform);
        }

        // Respawn the oranges at their original positions (if needed)
        for (int i = 0; i < orangePositions.Count; i++)
        {
            Instantiate(orangePrefab, orangePositions[i], Quaternion.identity, itemsParent.transform);
        }
    }
}
