using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] Obstacles;
  
    public float minRange = 0f;
    public float maxRange = 5f;

    private void OnEnable()
    {
        Invoke(nameof(SpawnObject), Random.Range(minRange, maxRange));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void SpawnObject()
    {  
        //pick a random sprite from the obstacles array
        int randomIndex = Random.Range(0, Obstacles.Length);
        GameObject randomObstacle = Obstacles[randomIndex];

        // to get the spawner object position and apear at that position while preserving the y axis 
        Vector3 newPosition = randomObstacle.transform.position;
        newPosition.x = transform.position.x;
        randomObstacle.transform.position = newPosition;

        Instantiate(randomObstacle);
         
        Invoke(nameof(SpawnObject), Random.Range(minRange, maxRange));
    }

}
