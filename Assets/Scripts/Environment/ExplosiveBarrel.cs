using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public Sprite explosion;
    public BoxCollider2D collider;
    [SerializeField]float timeofExplosion= 0.3f;
    [SerializeField]int ExplosionDamage = 50;
    float timer=0;
    bool Exploded;
    bool damageDone;

    public SpriteRenderer Sr;
    Health health;
    //GameObject Healthbar;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        Exploded = false;
        damageDone = false;

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
