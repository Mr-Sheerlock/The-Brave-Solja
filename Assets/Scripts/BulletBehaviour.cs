using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public GameObject HitEffect;

    float TimeSinceShot;

    int damage;

    void SetDamage(int x)
    {
        damage = x;
    }

    private void Start()
    {
        TimeSinceShot = 0f;
        damage = 1;
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
            GameObject lol = Instantiate(HitEffect, transform.position, transform.rotation);

            //CallBack?????2

            Destroy(gameObject);  
        }
        //else if (other.tag == "Enemy")
        //{

        //}
    }
}
