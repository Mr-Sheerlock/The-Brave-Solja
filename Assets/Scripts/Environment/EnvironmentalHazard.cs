using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalHazard : MonoBehaviour
{
    public int Damage;
    float timer;

    void Start()
    {
        timer = 0;
        Damage = 20;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
        
    //}
    private void OnTriggerEnter2D(Collider2D other)
    { 

        if (other.GetComponent<Health>() && timer>1f)
        {
            other.GetComponent<Health>().TakeDamage(Damage);
            timer = 0;
        }
    }
}
