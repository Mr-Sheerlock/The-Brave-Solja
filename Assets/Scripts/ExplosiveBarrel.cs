using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public Sprite explosion;
    public BoxCollider2D collider;
    float timeofExplosion;
    float timer;
    bool Exploded;
    bool damageDone;
    int ExplosionDamage;

    public SpriteRenderer Sr;
    Health health;
    //GameObject Healthbar;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        Exploded = false;
        damageDone = false;

        timer = 0;
        timeofExplosion = 0.3f;
        ExplosionDamage = 50;

    }
    private void Update()
    {
        if (Exploded)
        {
            if (!damageDone)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 6f, 1 << 8);

                foreach (Collider2D collider in colliders)
                {
                    if (collider.GetComponent<Health>())
                    {
                        collider.GetComponent<Health>().TakeDamage(ExplosionDamage);
                    }
                }
                damageDone = true;
            }
            
            timer += Time.deltaTime;
            if (timer > timeofExplosion)
            {
                Destroy(gameObject);
            }
        }

        // Update is called once per frame
    }

        public void Die()
        {
            collider.enabled = false;
            Exploded = true;
            Sr.sprite = explosion;
            transform.localScale = new Vector2(10, 10);
        }

}
