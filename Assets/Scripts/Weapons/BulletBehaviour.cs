using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public GameObject HitEffect;

    float TimeSinceShot;

    public int damage;

    public void SetDamage(int x)
    {
        damage = x;
    }

   
    private void Awake()
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

        //GameObject lol = Instantiate(HitEffect, transform.position, transform.rotation);

        //CallBack?????2


        GameObject lol = Instantiate(HitEffect, transform.position, transform.rotation);
        if (other.collider.GetComponent<Health>())
        {
            other.collider.GetComponent<Health>().TakeDamage(damage);
        }
        Destroy(gameObject);  
    }

    
    public void Collide()
    {
        GameObject lol = Instantiate(HitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }


}
