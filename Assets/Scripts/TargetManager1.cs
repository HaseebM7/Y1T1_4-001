using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TargetingManager1 : MonoBehaviour
{
    public List<Target> targets; // List of targets
    public GameObject player; // Reference to the player
    public float moveSpeed = 3f; // Speed of the player
    public float targetReachDistance = 1f; // Distance to reach the target
    public float maxDistanceForRed = 5f; // Max distance at which target starts to turn red

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

            // Add or find a TextMesh to display the value above the target
            TextMesh textMesh = obj.GetComponentInChildren<TextMesh>();
            if (textMesh == null)
            {
                // If the target doesn't have a TextMesh, create one as a child
                GameObject textObject = new GameObject("TargetValueText");
                textObject.transform.SetParent(obj.transform);
                textObject.transform.localPosition = new Vector3(0, 2, 0); // Position it above the target
                textMesh = textObject.AddComponent<TextMesh>();
                textMesh.fontSize = 24; // Set font size
                textMesh.color = Color.red; // Set the default color
            }

            // Update the text to show the value
            textMesh.text =  value.ToString("F0"); // Display the value with no decimal places
        }
    }

    void MoveTowardsBestTarget()
    {
        // Get the target with the highest value
        Target bestTarget = targets.OrderByDescending(t => t.value).First();

        // Move towards the best target
        Vector3 direction = (bestTarget.gameObject.transform.position - player.transform.position).normalized;
        player.transform.position += direction * moveSpeed * Time.deltaTime;

        // Change the target's color based on how close the player is
        float distanceToTarget = Vector3.Distance(player.transform.position, bestTarget.gameObject.transform.position);
        ChangeTargetColor(bestTarget.gameObject, distanceToTarget);

        // Check if the player is close enough to the target
        if (distanceToTarget < targetReachDistance)
        {
            Destroy(bestTarget.gameObject); // Destroy the target
            targets.Remove(bestTarget); // Remove it from the list
        }
    }

    // Changes the target's color based on the distance to the player
    void ChangeTargetColor(GameObject target, float distance)
    {
        Renderer targetRenderer = target.GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            // Calculate how red the target should be (closer = redder)
            float colorFactor = Mathf.Clamp01(1 - (distance / maxDistanceForRed)); // Normalize distance to [0, 1]
            targetRenderer.material.color = new Color(colorFactor, 1 - colorFactor, 1 - colorFactor); // Interpolate from green to red
        }
    }
}
