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

    public SpriteRenderer Sr;
    Health health;
    //GameObject Healthbar;
    // Start is called before the first frame update
    void Start()
    {
        health=GetComponent<Health>();
        Exploded = false;
        
        timer = 0;
        timeofExplosion = 0.3f;

    }
    private void Update()
    {
        if (Exploded)
        {
            timer += Time.deltaTime;
            if(timer> timeofExplosion)
            {
                Destroy(gameObject);
            }
        }
        
    }

    // Update is called once per frame


    public void Die()
    {
        collider.enabled = false;
        Exploded = true;
        Sr.sprite = explosion;
        transform.localScale = new Vector2(10, 10);
    }

}
