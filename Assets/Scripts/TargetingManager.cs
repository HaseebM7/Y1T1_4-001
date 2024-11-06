using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class TargetingManager : MonoBehaviour
{
    public List<Target> targets = new List<Target>(); //list of targets
    public GameObject player; //Reference to player
    public float moveSpeed = 3f; //Speed of player
    public float targetReachDistance = 1f; //Distance to reach target
    // Start is called before the first frame update
    void Start()
    {
        FindTargets(); //Locate targets
    }

    // Update is called once per frame
    void Update()
    {
        if (targets.Count > 0)
        {
            MoveTowardshighestValue(); //Move towards target
        }
    }

    void FindTargets()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Target"))
        {
            targets.Add(new Target(obj, Random.Range(1f, 10f))); //Adds a random value
        }
    }

    void MoveTowardshighestValue()
    {
        Target highestValue = targets.OrderByDescending(t => t.value).First();
        
        if (highestValue != null)
        {
            //Highlights the target red
            Renderer targetRenderer = highestValue.gameObject.GetComponent<Renderer>();
            if (targetRenderer != null)
            {
                targetRenderer.material.color = Color.red;
            }
            //Moves to Target
            player.transform.position = Vector3.MoveTowards(player.transform.position, highestValue.gameObject.transform.position, moveSpeed * Time.deltaTime);
            //Checks distance to remove target
            if (Vector3.Distance(player.transform.position, highestValue.gameObject.transform.position) < targetReachDistance)
            {
                Destroy(highestValue.gameObject);
                targets.Remove(highestValue);
            }
        }
    }
}