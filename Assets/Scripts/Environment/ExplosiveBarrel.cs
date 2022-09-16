using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    
    
    [SerializeField]int ExplosionDamage = 50;
    [SerializeField]float  ExplosionRadius= 5;  //5 to 7 bkteero
    [SerializeField] GameObject [] ExplosionEffects;
    [SerializeField] AudioClip ExplosionSound;
    [SerializeField] LayerMask ExplosionLayer;
    bool exploded = false; //to prevent infinite stack OverFlow
    public void Explode()
    {
        if (!exploded)
        {

            exploded = true;
            //might need to change the layermask
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 6f, ExplosionLayer);

            foreach (Collider2D collider in colliders)
            {
                if (collider.GetComponent<Health>())
                {
                    if (collider.tag == "Player")
                    {
                        float MaxPlayerHealth = collider.GetComponent<Health>().GetMaxHealth();
                        if (MaxPlayerHealth < ExplosionDamage)
                        {
                            collider.GetComponent<Health>().TakeDamage(MaxPlayerHealth*0.5f);
                        }
                        else
                        {
                            collider.GetComponent<Health>().TakeDamage(ExplosionDamage);
                        }
                    }
                    else
                    {
                        collider.GetComponent<Health>().TakeDamage(ExplosionDamage);
                    }
                }
            }
            ParticleSystem ps;
            for (int i = 0; i < ExplosionEffects.Length; i++)
            {
                Instantiate(ExplosionEffects[i], transform.position, transform.rotation);
                //Set Particle Sizes
                ps = ExplosionEffects[i].GetComponent<ParticleSystem>();
                ps.startSize =ExplosionRadius;
            }
            AudioSource.PlayClipAtPoint(ExplosionSound, transform.position);
            Destroy(gameObject);
        }
    }

}
