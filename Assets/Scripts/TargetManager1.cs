using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TargetingManager1 : MonoBehaviour
{
    public List<Target> targets; // List of targets
    public GameObject player; // Reference to the player
    public float moveSpeed = 3f; // Speed of the player
    public float targetReachDistance = 1f; // Distance to reach the target

    void Start()
    {
        targets = new List<Target>();
        FindTargets(); // Find targets when the game starts
    }

    void Update()
    {
        if (targets.Count > 0)
        {
            MoveTowardsBestTarget(); // Move towards the best target each frame
        }
    }

    void FindTargets()
    {
        // Find all GameObjects with the "Target" tag
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Target");

        // Add each target to the list with a random value (you can adjust this logic)
        foreach (GameObject obj in targetObjects)
        {
            float value = Random.Range(1f, 10f); // Example value assignment
            targets.Add(new Target(obj, value));
        }
    }

    void MoveTowardsBestTarget()
    {
        // Get the target with the highest value
        Target bestTarget = targets.OrderByDescending(t => t.value).First();

        // Change the color of the best target to red
        Renderer targetRenderer = bestTarget.gameObject.GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            targetRenderer.material.color = Color.red;
        }

        // Move towards the best target
        Vector3 direction = (bestTarget.gameObject.transform.position - player.transform.position).normalized;
        player.transform.position += direction * moveSpeed * Time.deltaTime;

        // Check if the player is close enough to the target
        if (Vector3.Distance(player.transform.position, bestTarget.gameObject.transform.position) < targetReachDistance)
        {
            Destroy(bestTarget.gameObject); // Destroy the target
            targets.Remove(bestTarget); // Remove it from the list
        }
    }
}
