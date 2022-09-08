using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalHazard : MonoBehaviour
{
    [SerializeField] int Damage=20;
    float timer;

    void Start()
    {
        timer = 0;
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
