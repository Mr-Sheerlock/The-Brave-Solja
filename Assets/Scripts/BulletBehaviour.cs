using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    float TimeSinceShot;

    private void Start()
    {
        TimeSinceShot = 0f;
    }

    void Update()
    {
        TimeSinceShot += Time.deltaTime;
        if(TimeSinceShot >5f)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D( Collision2D other)
    {
        if (other.collider.tag == "Walls")
        {
            Debug.Log("LOLBullet");
            Destroy(gameObject);  
        }
        //else if (other.tag == "Enemy")
        //{

        //}
    }
}
