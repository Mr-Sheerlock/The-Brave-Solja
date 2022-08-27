using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    
   
    [SerializeField]int ExplosionDamage = 50;
    [SerializeField]float  ExplosionRadius= 5;  //5 to 7 bkteero
    [SerializeField] GameObject [] ExplosionEffects;


    public void Explode()
    {
        //might need to change the layermask
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 6f, 1 << 8);

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Health>())
            {
                collider.GetComponent<Health>().TakeDamage(ExplosionDamage);
            }
        }

        for(int i = 0; i < ExplosionEffects.Length; i++)
        {
            Instantiate(ExplosionEffects[i],transform.position,transform.rotation);
            //Set Particle Sizes
        }

        Destroy(gameObject);
    }

}
