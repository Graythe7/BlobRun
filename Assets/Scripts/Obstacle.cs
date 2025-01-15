using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float lifeTime = 5f;
    
    private void Update()
    {
        float speed = GameManager.Instance.gameSpeed;
        transform.position += Vector3.left * speed * Time.deltaTime;

        Destroy(gameObject, lifeTime);
    }
}
